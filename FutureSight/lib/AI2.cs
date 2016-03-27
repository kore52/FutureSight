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
            this.score = score;
            this.depth = depth;
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
        
        public MTGChoice FindNextEventChoiceResults(GameState game)
        {
            MTGEvent ev = game.GetNextEvent();
            
            // search choices related by next event
            foreach (var choice in ev.Choices)
            {
                GameState copiedGame = (GameState)Utilities.DeepCopy(game);
                
                // TODO : multi threading
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    var worker = new AIWorker();
                    worker.RunGame(choice, 0, 5000);
                
                    // record score
                    var gameId = 
                }
            }
            return null;
        }

        /*
        public static void Calculate(GameTree tree, int depth)
        {
            if (depth > (int)Depth.Max) { return; }

            if (tree == null)
            {
                tree = new GameTree();
            }

            var moveCandidates = Search(tree);
            
            /*
            tree.Node
/*            foreach (var move in moveCandidates)
            {
                // 探索した行動ごとに盤面を進める
                GameState stateAfterMove = DoMove(move, tree.Data);
                GameTree newTree = new GameTree(stateAfterMove);

                // 移動後の評価値を計算
                newTree.Score = Evaluate.evaluate(stateAfterMove);

                // 親ノードのスコアを更新
                tree.Score = Math.Max(tree.Score, newTree.Score);

                tree.Node.Add(newTree);

#if DEBUG
                // 木の状態を表示
                string sp = "";
                for (int c = 0; c < depth; c++) { sp += " "; }
                System.Diagnostics.Debug.Print(String.Format(sp + "Depth{0}->{1}: ActPly:{2}, Pri:{3}, Turn:{4}, Step:{5}",
                    depth, depth + 1,
                    stateAfterMove.GetActivePlayerNumber(),
                    stateAfterMove.Priority,
                    (int)stateAfterMove.ElapsedTurns,
                    (int)stateAfterMove.Step));

                // 盤面の状態を表示
                System.Diagnostics.Debug.Print(String.Format(sp + "[me:H:{0}, P:{1}], [op:H:{2}, P:{3}]",
                    Utilities.Join(stateAfterMove.Players[0].Hand),
                    String.Join(",", stateAfterMove.Players[0].Permanents.Select(item => item.ID).ToArray()),
                    Utilities.Join(stateAfterMove.Players[1].Hand),
                    String.Join(",", stateAfterMove.Players[1].Permanents.Select(item => item.ID).ToArray())));
#endif

                // 子ノードの行動探索
                Calculate(newTree, depth + 1);
            }
        }*/
       
    }
    
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

        public void EvaluateGame(MTGChoiceResults choice)
        {
            scoreBoard[game.ID] = RunGame(choice, 0, 10000);
        }

        private AIScore RunGame(MTGChoiceResults choice, int depth, long maxTime)
        {
            // キューに溜まっているイベントを処理
            if (choice != null)
            {
                game.ExecuteNextEvent(choice);
            }

            // スレッドの終了条件を満たしていれば評価値を返す
            if (stopWatch.ElapsedMilliseconds > maxTime)
            {
                var aiScore = new AIScore(game.Score, depth);
                return aiScore;
            }

            while (!game.IsFinished)
            {
                if (game.Events.Count > 0)
                {
                    game.ExecutePhase();

                    var mtgevent = game.GetNextEvent();
                    mtgevent.Action.ExecuteEvent(game, mtgevent);
                }
            }

            return new AIScore();
        }
    }
}



