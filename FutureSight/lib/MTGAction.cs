using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{

    /// イベントに対するアクション定義の基底クラス
    interface MTGAction
    {
        void DoAction(GameState game);
        void UndoAction(GameState game);
    }

    /// プレイヤーが土地をプレイするアクション
    public class PlayLandAction : MTGAction
    {
        public PlayLandAction(MTGCard card, MTGPlayer player)
        {
            this.card = card;
            this.player = player;
        }

        public void DoAction(GameState game)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(player.Hand.Contains(card));
#endif
            permanent = new MTGPermanent(card);
            player.Permanents.Add(permanent);
            player.Hand.Remove(card);
        }

        public void UndoAction(GameState game)
        {
            player.Permanents.Remove(permanent);
            player.Hand.Add(card);
        }

        private MTGPlayer    player;
        private MTGCard      card;
        private MTGPermanent permanent;
    }
}
