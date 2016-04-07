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

            // �I�������X�g����ł��]���l�̍����I������Ԃ�
            
            List<object> resultChoice = new List<object>();
            resultChoice.Add(aiChoiceResults.Aggregate((r1, r2) => r1.Score.score > r2.Score.score ? r1 : r2).Max());
            return resultChoice;
        }
    }
    
    /// <summary>
    /// �T���̕��U�����p�̃��[�J�[�N���X
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

        // �]���̕K�v������X�e�b�v�ƂȂ��X�e�b�v�𔻒�
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
            // �L���[�ɗ��܂��Ă���C�x���g������
            if (nextChoiceResults != null)
            {
                game.ExecuteNextEvent(nextChoiceResults);
            }
            
            // �]���̏I�������𖞂����Ă���΂������ɕ]���l��Ԃ�
            if (stopWatch.ElapsedMilliseconds > maxTime)
            {
                var aiScore = new AIScore(game.Score, depth);
                return aiScore;
            }
            
            // ���[�v���I����������̓Q�[�����I�����邩�A���Ԃ��I�[�o�[���邩
            while (!game.IsFinished())
            {
                AIScore bestScore = new AIScore();
                if (game.Events.Count > 0)
                {
                    // �t�F�C�Y�̏���
                    game.ExecutePhase();
                    
                    // �X�R�A�̕]�����K�v�ȃt�F�C�Y�i�X�e�b�v�j�ł��邩
                    if (ShouldCache())
                    {
                        // ���łɕ]���l���v�Z����Ă���Ֆʂ̏ꍇ�͕]�����ȗ�
                        AIScore returnBestScore;
                        var gameID = game.ID + pruneScoreBest;
                        bestScore = scoreBoard[(long)gameID];
                        if (bestScore == null)
                        {
                            // �Ֆʂ��]������Ă��Ȃ��ꍇ�͕]�����s���]���l��Ԃ�
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
                
                // �C�x���g�̎��s
                var mtgevent = game.GetNextEvent();
                mtgevent.Action.ExecuteEvent(game, mtgevent);
                if (!mtgevent.HasChoice())
                {
                    game.ExecuteNextEvent();
                    continue;
                }

                // �C�x���g�̑I�����̎擾
                var choiceResultList = mtgevent.GetChoiceResults(game);
                
                // �I�������P�������݂��Ȃ��Ȃ�΂��̂܂܃C�x���g������
                if (choiceResultList.Count == 1)
                {
                    game.ExecuteNextEvent(choiceResultList);
                    continue;
                }
                
                // �I��������������ꍇ�A���ꂼ��ɑ΂��ĕ]�����s��
                bool best = (game.ScorePlayer == mtgevent.Player);
                long end = stopWatch.ElapsedMilliseconds;
                long slice = (maxTime - end) / choiceResultList.Count;
                foreach (var choiceResult in choiceResultList)
                {
                    // �c�莞�Ԃ̐ݒ�
                    end += slice;
                    
                    // ���̑I������������ꍇ�̃X�R�A���v�Z
                    var subtreeScore = RunGame(new MTGChoiceResults(choiceResult), ref pruneScoreBest, ref pruneScoreWorst, depth + 1, end);
                    
                    // �]���l�̌v�Z
                    // �]�����s���v���C���[�ƃC�x���g���s���v���C���[������ł���ꍇ�͑傫�ȕ]���l�A
                    // �ΐ푊��̕]���l�Ȃ�Ε��̕]���l�Ƃ��Čv�Z����
                    if (best)
                    {
                        bestScore.score = (subtreeScore.score > bestScore.score) ? subtreeScore.score : bestScore.score;
                        
                        // �K��X�R�A�ȉ��Ȃ炱��ȏ�͕]�����Ȃ�
                        if (bestScore.score > pruneScoreWorst) break;
                    } else {
                        bestScore.score = (subtreeScore.score < bestScore.score) ? subtreeScore.score : bestScore.score;
                        
                        // �K��X�R�A�ȉ��Ȃ炱��ȏ�͕]�����Ȃ�
                        if (bestScore.score < pruneScoreBest) break;
                    }
                }
                
                // �Q�[���������߂�
                game.Restore();
                return bestScore;
            }

            // �Q�[���I��
            var finScore = new AIScore(game.Score, depth);
            game.Restore();
            return finScore;
        }
    }
}



