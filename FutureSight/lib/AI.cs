using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

using Move = System.String;

/*
plyr,zone_id,object_id,(cast|play|activate),
plyr,zone_id,object_id,activate,ab_id,[trgt_id,...]
plyr,effect_id,
plyr,attack,[attk_creature:(plyr|pw)][,...]
plyr,block,[blck_creature:(attk_creature)][,...]
plyr,blckdet,[attk_creature|
*/

namespace FutureSight.lib
{
    public class AI
    {
        // 思考ルーチン
        public static void Calculate(GameTree tree, int depth)
        {
            if (depth > (int)Depth.Max) { return; }

            if (tree == null)
            {
                tree = new GameTree();
            }

            // 優先権を持っているプレイヤーの次の行動の候補を探索
            var moveCandidates = Search(tree.Data);
            foreach (var move in moveCandidates)
            {
                // 探索した行動ごとに盤面を進める
                GameState stateAfterMove = DoMove(move, tree.Data);

                // 移動後の評価値を計算
                stateAfterMove.EvalScore = Evaluate.evaluate(stateAfterMove);

                // 親ノードのスコアを更新
                tree.Data.EvalScore = Math.Max(tree.Data.EvalScore, stateAfterMove.EvalScore);

                GameTree newGT = new GameTree(stateAfterMove);
                tree.Node.Add(newGT);

#if DEBUG
                // 木の状態を表示
                string sp = "";
                for (int c=0; c < depth; c++) { sp += " "; }
                System.Diagnostics.Debug.Print(String.Format(sp + "Depth:{0}->{1}: ActPly:{2}, Pty:{3}, Turn:{4}, Step:{5}",
                    depth, depth + 1,
                    stateAfterMove.GetActivePlayerNumber(), 
                    stateAfterMove.priority,
                    (int)stateAfterMove.turns,
                    (int)stateAfterMove.step));

                // 盤面の状態を表示
                System.Diagnostics.Debug.Print(String.Format(sp + "[me:H:{0}, P:{1}], [op:H:{2}, P:{3}]",
                    Utilities.Join(stateAfterMove.Players[0].Hand),
                    String.Join(",", stateAfterMove.Players[0].Permanents.Select(item => item.ID).ToArray()),
                    Utilities.Join(stateAfterMove.Players[1].Hand),
                    String.Join(",", stateAfterMove.Players[1].Permanents.Select(item => item.ID).ToArray())));
#endif

                // 子ノードの行動探索
                Calculate(newGT, depth + 1);
            }
        }
        
        // 行動パターンの列挙
        public static List<Move> Search(GameState state)
        {
            var nextMove = new List<Move>();

            Player player = state.Players[priority];

            for (int i = 0; i < player.Hand.Count; i++)
            {
                Card item = CardDB.GetInstance().get(player.Hand[i]);
                switch (state.GetCurrentStep())
                {
                    case GamePhase.UntapStep: break;
                    case GamePhase.UpkeepStep:
                    case GamePhase.DrawStep:
                        if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        break;
                    case GamePhase.Main1:
                        if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }

                        if (player.ID == GetActivePlayer().ID)
                        {
                            if (item.CardType.HasFlag(CardType.Land) && canPlayLand) { nextMove.Add(GetActivePlayerNumber() + ":play:" + i); }
                            if (item.CardType.HasFlag(CardType.Creature) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Sorcery) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Enchantment) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Artifact) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Planeswalker) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        }
                        break;
                    case GamePhase.PreCombatStep:
                    case GamePhase.DeclareAttackerStep:
                    case GamePhase.DeclareBlockerStep:
                    case GamePhase.CombatDamageStep:
                    case GamePhase.EndOfCombatStep:
                        if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        break;
                    case GamePhase.Main2:
                        if (item.CardType.HasFlag(CardType.Land) && canPlayLand) { nextMove.Add(GetActivePlayerNumber() + ":play:" + i); }

                        if (player.ID == GetActivePlayer().ID)
                        {
                            if (item.CardType.HasFlag(CardType.Creature) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Sorcery) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Enchantment) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Artifact) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Planeswalker) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        }
                        break;
                    case GamePhase.EndStep:
                        if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        break;
                    case GamePhase.CleanupStep: break;

                }

            }
            nextMove.Add(priority + ":none");

#if DEBUG
            string c = "";
            foreach (var nm in nextMove) { c += "[" + nm + "]"; }
            System.Diagnostics.Debug.Print(c);
#endif
            return nextMove;
        }
        
        private static List<Move> Search()
        {
        }
        
        // 1手進めた盤面を返す
        public static GameState DoMove(Move move, GameState previousState)
        {
        }
    }
}



