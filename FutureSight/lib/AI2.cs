using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

using Move = System.String;

namespace FutureSight.lib
{
    public class AIScore
    {
        public int score { get; set; }
        public int depth { get; set; }

        public AIScore()
        {
            this.score = 0;
            this.depth = 0;
        }

        public AIScore(int score, int depth)
        {
            this.score = Math.Max(Math.Min(score, int.MaxValue), -int.MaxValue);
            this.depth = depth;
        }
        
        public AIScore GetScore(int depthIncrements)
        {
            return new AIScore(score, depth + depthIncrements);
        }
    }
    
    public class AI2
    {
        private Stopwatch stopWatch;
        private Dictionary<int, GameState> scoreBoard;

        public AI2()
        {
            stopWatch = new Stopwatch();
        }
        
        public List<object> FindNextEventChoiceResults(GameState game, MTGPlayer player)
        {
            game.Log(player, "FindNextEventChoiceResults");
            
            MTGEvent ev = game.GetNextEvent();
            var scoreBoard = new Dictionary<long, AIScore>();
            
            var aiChoiceResults = new List<MTGChoiceResults>();
            
            // search choices related by next event
            foreach (var choice in ev.GetChoiceResults(game))
            {
                var achoice = new MTGChoiceResults(choice);
                aiChoiceResults.Add(achoice);
                
                GameState copiedGame = (GameState)Utilities.DeepCopy(game);
                
                // TODO : multi threading
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    var worker = new AIWorker(0, game, scoreBoard);
                    worker.EvaluateGame(achoice, 5000);
                }
            }

            // 選択肢リストから最も評価値の高い選択肢を返す
            
            List<object> resultChoice = new List<object>();
            resultChoice.Add(aiChoiceResults.Aggregate((r1, r2) => r1.Score.score > r2.Score.score ? r1 : r2).Max());
            return resultChoice;
        }
    }
    
    /// <summary>
    /// 探索の分散処理用のワーカークラス
    /// </summary>
    public class AIWorker
    {
        private int id;
        private GameState game;
        private Dictionary<long, AIScore> scoreBoard;
        private Stopwatch stopWatch;

        public AIWorker(int id, GameState game, Dictionary<long, AIScore> scoreBoard)
        {
            this.id = id;
            this.game = game;
            this.scoreBoard = scoreBoard;

            stopWatch = new Stopwatch();
            stopWatch.Reset();
            stopWatch.Start();
        }

        public void EvaluateGame(MTGChoiceResults choice, int maxTime)
        {
            int min = int.MinValue;
            int max = int.MaxValue;
            scoreBoard[game.ID] = RunGame(choice, ref min, ref max, 0, maxTime);
        }

        // 評価の必要があるステップとないステップを判定
        private bool ShouldCache()
        {
            switch (game.Phase.Type)
            {
            case MTGPhaseType.FirstMain:
            case MTGPhaseType.EndCombat:
            case MTGPhaseType.Cleanup:
                return game.Step == MTGStep.NextPhase;
            }
            return false;
        }
        
        private AIScore RunGame(MTGChoiceResults nextChoiceResults, ref int pruneScoreBest, ref int pruneScoreWorst, int depth, long maxTime)
        {
            // キューに溜まっているイベントを処理
            if (nextChoiceResults != null)
            {
                game.ExecuteNextEvent(nextChoiceResults);
            }
            
            // 評価の終了条件を満たしていればただちに評価値を返す
            if (stopWatch.ElapsedMilliseconds > maxTime)
            {
                var aiScore = new AIScore(game.Score, depth);
                return aiScore;
            }
            
            // ループを終了する条件はゲームが終了するか、時間をオーバーするか
            while (!game.IsFinished())
            {
                AIScore bestScore = new AIScore();
                if (game.Events.Count > 0)
                {
                    // フェイズの処理
                    game.ExecutePhase();
                    
                    // スコアの評価が必要なフェイズ（ステップ）であるか
                    if (ShouldCache())
                    {
                        // すでに評価値が計算されている盤面の場合は評価を省略
                        AIScore returnBestScore;
                        var gameID = game.ID + pruneScoreBest;
                        bestScore = scoreBoard[(long)gameID];
                        if (bestScore == null)
                        {
                            // 盤面が評価されていない場合は評価を行い評価値を返す
                            returnBestScore = RunGame(null, ref pruneScoreBest, ref pruneScoreWorst, depth, maxTime);
                            returnBestScore.score = -returnBestScore.score;
                            scoreBoard[(long)gameID] = returnBestScore;
                        }
                        else
                        {
                            returnBestScore = bestScore.GetScore(depth);
                        }
                        return returnBestScore;
                    }
                    continue;
                }
                
                // イベントの実行
                var mtgevent = game.GetNextEvent();
                mtgevent.Action.ExecuteEvent(game, mtgevent);
                if (!mtgevent.HasChoice())
                {
                    game.ExecuteNextEvent();
                    continue;
                }

                // イベントの選択肢の取得
                var choiceResultList = mtgevent.GetChoiceResults(game);
                
                // 選択肢が１つしか存在しないならばそのままイベントを処理
                if (choiceResultList.Count == 1)
                {
                    game.ExecuteNextEvent(choiceResultList);
                    continue;
                }
                
                // 選択肢が複数ある場合、それぞれに対して評価を行う
                bool best = (game.ScorePlayer == mtgevent.Player);
                long end = stopWatch.ElapsedMilliseconds;
                long slice = (maxTime - end) / choiceResultList.Count;
                foreach (var choiceResult in choiceResultList)
                {
                    // 残り時間の設定
                    end += slice;
                    
                    // その選択肢を取った場合のスコアを計算
                    var subtreeScore = RunGame(new MTGChoiceResults(choiceResult), ref pruneScoreBest, ref pruneScoreWorst, depth + 1, end);
                    
                    // 評価値の計算
                    // 評価を行うプレイヤーとイベントを行うプレイヤーが同一である場合は大きな評価値、
                    // 対戦相手の評価値ならば負の評価値として計算する
                    if (best)
                    {
                        bestScore.score = (subtreeScore.score > bestScore.score) ? subtreeScore.score : bestScore.score;
                        
                        // 規定スコア以下ならこれ以上は評価しない
                        if (bestScore.score > pruneScoreWorst) break;
                    } else {
                        bestScore.score = (subtreeScore.score < bestScore.score) ? subtreeScore.score : bestScore.score;
                        
                        // 規定スコア以下ならこれ以上は評価しない
                        if (bestScore.score < pruneScoreBest) break;
                    }
                }
                
                // ゲームを巻き戻す
                game.Restore();
                return bestScore;
            }

            // ゲーム終了
            var finScore = new AIScore(game.Score, depth);
            game.Restore();
            return finScore;
        }
    }
}



