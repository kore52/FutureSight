using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics.Stopwatch;

using Move = System.String;

namespace FutureSight.lib
{
    public class AIScore
    {
        public int score { get; set; }
        public int depth { get; set; }
    }
    
    public class AI2
    {
        public AI2()
        {
            stopWatch = new StopWatch();
        }
        
        public MTGChoice FindNextEventChoiceResults(GameState game)
        {
            MTGEvent ev = game.GetNextEvent();
            
            // search choices related by next event
            foreach (var choice in ev.GetChoices())
            {
                GameState copiedGame = (GameState)Utilities.DeepCopy(game);
                
                // TODO : multi threading
                {
                    stopWatch.StartNew();
                    var score = RunGame(choice, DepthZero, 5000);
                
                    // record score
                    var gameId = 
                }
            }
            return null;
        }
        
        public AIScore RunGame(object choice, int depth, int maxTime)
        {
            var aScore = new AIScore();
            if (maxTime < stopWatch.ElapsedMilliseconds) {
                aScore.score = 
                return score;
            }
            var score = RunGame(choice, depth + 1, maxTime);
            return score;
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
            }*/
        }
        
        // Search all possible moves
        private static List<GameState> Search(GameState sourceGame)
        {
            /*
            if (sourceGame.HasNextEvent())
            */
            return null;
        }
        
        // Forward game
        public static GameState DoMove(Move move, GameState previousState)
        {
            // 盤面情報のコピー
            GameState state = (GameState)previousState.DeepCopy();

            string[] moveElement = move.Split(':');
            int indexOfPlayer = int.Parse(moveElement[0]);

            // ターン起因処理

            switch (moveElement[1])
            {
                // 優先権をパスし、次のプレイヤーに優先権を渡す
                case "none":
                    if (state.TurnOrder.Count > state.Priority + 1)
                    {
                        state.Priority++;
                    }
                    else {
                        state.Priority = state.TurnOrder[0];
                        if (state.Step != GamePhase.CleanupStep)
                        {
                            state.Step++;
                        }
                        else {
                            state.Step = GamePhase.UntapStep;
                            state.ElapsedTurns++;
                        }
                    }
                    break;

                // 土地のプレイ
                case "play":
                    int indexOfHand = int.Parse(moveElement[2]);
                    // パーマネントに登録し、手札から除外する
                    int hid = state.Players[indexOfPlayer].Hand[indexOfHand];
                    state.Players[indexOfPlayer].Permanents.Add(new Permanent(indexOfHand));
                    state.Players[indexOfPlayer].Hand.RemoveAt(hid);
                    break;
            }

            // 行った動作を保存
            state.CurrentMove = move;
            CheckStateBasedAction(state);


            return state;
        }

        // マナコストを満たしているかチェック
        private static bool IsManaCostSatisfied(string cost, MTGPlayer player)
        {
            bool result = false;

            // calc max mana in manapool
            int max = player.ManaPool.Sum();
            List<int> costList = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
            List<int> tmp = new List<int>(player.ManaPool);

            var match = cost.Split('{');
            foreach (var m in match)
            {
                var ms = m;
                if (m.Length <= 0) { continue; } else { ms = m.Substring(0, m.Length - 1); }

                switch (ms)
                {
                    case "W": costList[(int)Color.White]++; break;
                    case "U": costList[(int)Color.Blue]++; break;
                    case "B": costList[(int)Color.Black]++; break;
                    case "R": costList[(int)Color.Red]++; break;
                    case "G": costList[(int)Color.Green]++; break;
                    case "C": costList[(int)Color.Colorless]++; break;
                    case "X":
                    case "Y":
                    case "Z":
                        break;
                    default:
                        // 不特定マナ
                        costList[(int)Color.Generic] += int.Parse(ms);
                        break;
                }
            }

            tmp[0] = (tmp[0] - costList[0] >= 0) ? tmp[0] - costList[0] : 0;
            tmp[1] = (tmp[1] - costList[1] >= 0) ? tmp[1] - costList[1] : 0;
            tmp[2] = (tmp[2] - costList[2] >= 0) ? tmp[2] - costList[2] : 0;
            tmp[3] = (tmp[3] - costList[3] >= 0) ? tmp[3] - costList[3] : 0;
            tmp[4] = (tmp[4] - costList[4] >= 0) ? tmp[4] - costList[4] : 0;
            tmp[5] = (tmp[5] - costList[5] >= 0) ? tmp[5] - costList[5] : 0;

            if (costList[(int)Color.White] <= player.ManaPool[(int)Color.White] &&
                costList[(int)Color.Blue] <= player.ManaPool[(int)Color.Blue] &&
                costList[(int)Color.Black] <= player.ManaPool[(int)Color.Black] &&
                costList[(int)Color.Red] <= player.ManaPool[(int)Color.Red] &&
                costList[(int)Color.Green] <= player.ManaPool[(int)Color.Green] &&
                costList[(int)Color.Colorless] <= player.ManaPool[(int)Color.Colorless] &&
                costList[(int)Color.Generic] <= tmp.Sum())
            {
                result = true;
            }
            return result;
        }
        
        private StopWatch stopWatch;
        private Dictionary<int, GameState> scoreBoard;
    }
    
    public class AIWorker
    {
        private int id;
        private GameState game;
        private Dictionary<int, GameState> scoreBoard;
        
        public AIWorker(int id, GameState game, Dictionary<int, GameState> scoreBoard)
        {
            this.id = id;
            this.game = game;
            this.scoreBoard = scoreBoard;
        }
        
        public AIScore RunGame
    }
}



