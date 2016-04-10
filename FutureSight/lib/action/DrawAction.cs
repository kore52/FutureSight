using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class DrawAction : MTGAction
    {
        private MTGPlayer player;
        private List<MTGCard> drawnCards;
        private int amount;

        public DrawAction(MTGPlayer player, int amount)
        {
            this.player = player;
            this.amount = amount;
            this.drawnCards = new List<MTGCard>();
        }

        // アクションを行う
        public override void DoAction(MTGGame game)
        {
            int score = 0;
            for (int i = 0; i < amount; i++)
            {
                if (player.Library.Count == 0)
                {
                    if (!player.HasState(MTGPlayerState.CannotLoseGame))
                        // rule 104.3c ライブラリーが空の状態でカードを引くと敗北
                        game.DoAction(new LoseGameAction(player, LoseGameAction.LoseGameReason.Draw));
                    break;
                }
                
                var card = player.Library[0];
                player.Hand.Add(card);
                player.Library.Remove(card);
                drawnCards.Add(card);
                
                score += card.Score;
            }
            SetScore(player, score);
            game.SetStateCheckRequired();
        }

        // アクションを戻す
        public override void UndoAction(MTGGame game)
        {
            for (int i = drawnCards.Count-1; i < 0; i--)
            {
                var card = drawnCards[i];
                player.Hand.Remove(card);
                player.Library.AddToTop(card);
            }
        }
    }
}