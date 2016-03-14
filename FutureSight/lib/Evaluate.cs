using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    enum Value
    {
        None = 0,
        Lose = -32768,
        Win = 32768
    }

    [Serializable()]
    class Evaluate
    {
        public static readonly int[] scoreByPermanentType = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // 評価関数
        public static int evaluate(GameState state)
        {
            int score = 0;

            foreach (PlayerState player in state.Players)
            {
                if (player.ID == state.GetActivePlayer().ID)
                {
                    // 手札の数の評価
                    score += player.Hand.Count * 10;

                    // 戦場のパーマネントの数の評価
                    foreach (var perm in player.Permanents)
                    {
                        if (perm.PermanentType.HasFlag(PermanentType.Land)) { score += 5; }
                        if (perm.PermanentType.HasFlag(PermanentType.Creature)) { score += 20; }
                        if (perm.PermanentType.HasFlag(PermanentType.Artifact)) { score += 10; }
                        if (perm.PermanentType.HasFlag(PermanentType.Enchantment)) { score += 10; }
                        if (perm.PermanentType.HasFlag(PermanentType.Planeswalker)) { score += 100; }
                    }

                    // ライフの評価
                    if (player.Life <= 0)
                    {
                        score += -(int)Value.Lose;
                    } else
                    {
                        score += player.Life * 10;
                    }
                    
                }
                else {

                    // 手札の数の評価
                    score -= state.GetActivePlayer().Hand.Count * 10;

                    // 戦場のパーマネントの数の評価
                    foreach (var perm in player.Permanents)
                    {
                        if (perm.PermanentType.HasFlag(PermanentType.Land)) { score -= 5; }
                        if (perm.PermanentType.HasFlag(PermanentType.Creature)) { score -= 20; }
                        if (perm.PermanentType.HasFlag(PermanentType.Artifact)) { score -= 10; }
                        if (perm.PermanentType.HasFlag(PermanentType.Enchantment)) { score -= 10; }
                        if (perm.PermanentType.HasFlag(PermanentType.Planeswalker)) { score -= 100; }
                    }

                    // ライフの評価
                    if (player.Life <= 0)
                    {
                        score += (int)Value.Win;
                    }
                    else
                    {
                        score -= player.Life * 10;
                    }
                }
            }


            return score;
        }

        public int Score { get; set; } = 0;
    }
}
