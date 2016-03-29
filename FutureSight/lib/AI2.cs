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
        
        public GetScore(int depthIncrements)
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
        
        public MTGChoice FindNextEventChoiceResults(GameState game)
        {
            MTGEvent ev = game.GetNextEvent();
            var scoreBoard = new Dictionary<long, AIScore>();
            
            var aiChoiceResults = new List<MTGChoiceResults>();
            
            // search choices related by next event
            foreach (var choice in ev.Choices)
            {
                aiChoiceResults.Add(choice);
                
                GameState copiedGame = (GameState)Utilities.DeepCopy(game);
                
                // TODO : multi threading
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    var worker = new AIWorker(0, game, scoreBoard);
                    worker.EvaluateGame(choice, 5000);
                }
            }
            
            // �I�������X�g����ł��]���l�̍����I������Ԃ�
            MTChoice resultChoice = aiChoiceResults.Select(item => item.Score).Max();
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
            scoreBoard[game.ID] = RunGame(choice, int.MinValue, int.MaxValue, 0, maxTime);
        }

        // �]���̕K�v������X�e�b�v�ƂȂ��X�e�b�v�𔻒�
        private bool ShouldCache()
        {
            switch (game.Phase.GetType())
            {
            case MTGPhaseType.FirstMain:
            case MTGPhaseType.EndOfCombat:
            case MTGPhaseType.Cleanup:
                return game.Step == MTGStep.NextPhase;
                break;
            default:
                break;
            }
            return false;
        }
        
        private AIScore RunGame(MTGChoiceResults nextChoiceResults, int pruneScoreBest ref, int pruneScoreWorst ref, int depth, long maxTime)
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
            while (!game.IsFinished)
            {
                AIScore bestScore;
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
                            returnBestScore = RunGame(null, pruneScoreBest, pruneScoreWorst, depth, maxTime);
                            scoreBoard[(long)gameID] = -returnBestScore.score;
                        }
                        else
                        {
                            returnBestScore = bestScore.getScore(depth);
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
                    game.ExecutNextEvent();
                    continue;
                }

                // �C�x���g�̑I�����̎擾
                var choiceResultList = mtgevent.GetChoiceResults(game);
                
                // �I�������P�������݂��Ȃ��Ȃ�΂��̂܂܃C�x���g������
                if (choiceResultList.Count == 1)
                {
                    game.ExecuteNextEvent(choiceresultList.First);
                    continue;
                }
                
                // �I��������������ꍇ�A���ꂼ��ɑ΂��ĕ]�����s��
                bool best = (game.ScorePlayer == event.Player);
                long slice = (maxTime - stopWatch.ElapsedMilliseconds) / choiceResultList.Count;
                foreach (var choiceResult in choiceResultList)
                {
                    // �c�莞�Ԃ̐ݒ�
                    end += slice;
                    
                    // ���̑I������������ꍇ�̃X�R�A���v�Z
                    var subtreeScore = RunGame(choiceResult, pruneScoreBest, pruneScoreWorst, depth + 1, end);
                    
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



