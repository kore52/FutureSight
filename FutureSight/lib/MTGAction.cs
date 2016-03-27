using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    /// イベントに対するアクション定義の基底クラス
    public class MTGAction
    {
        private MTGPlayer scorePlayer;
        private int score;

        // アクションを行う
        public void DoAction(GameState game)
        {
        }

        // アクションを戻す
        public void UndoAction(GameState game)
        {
        }

        // アクションスコアをセット
        public void SetScore(MTGPlayer scorePlayer, int score)
        {
            this.scorePlayer = scorePlayer;
            this.score = score;
        }

        // アクションに対するスコアを取得
        // アクションを行うプレイヤーが評価するプレイヤーと同じであれば加算、対戦相手であれば減算
        public int GetScore(MTGPlayer player)
        {
            return (player.ID == scorePlayer.ID) ? score : -score;
        }
    }

    /// プレイヤーが土地をプレイするアクション
    public class PlayLandAction : MTGAction
    {
        private MTGPlayer player;
        private MTGCard card;
        private MTGPermanent permanent;

        public PlayLandAction(MTGCard card, MTGPlayer player)
        {
            this.card = card;
            this.player = player;
        }

        public new void DoAction(GameState game)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(player.Hand.Contains(card));
#endif
            permanent = new MTGPermanent(card);
            player.Permanents.Add(permanent);
            player.Hand.Remove(card);
        }

        public new void UndoAction(GameState game)
        {
            player.Permanents.Remove(permanent);
            player.Hand.Add(card);
        }
    }

    /// <summary>
    /// ExecuteFirstEventAction
    /// </summary>
    public class ExecuteFirstEventAction : MTGAction
    {
        private MTGChoiceResults choiceResults;
        private MTGEvent firstEvent;

        public ExecuteFirstEventAction(MTGChoiceResults choiceResults)
        {
            this.choiceResults = choiceResults;
        }

        public new void DoAction(GameState game)
        {
            firstEvent = game.Events.First.Value;
            game.ExecuteEvent(firstEvent, choiceResults);
        }

        public new void UndoAction(GameState game)
        {
            game.Events.AddFirst(firstEvent);
        }
    }

    public class RemoveFromBattleField : MTGAction
    {
        private MTGPermanent permanent;
        private LocationType toLocation;

        public RemoveFromBattleField(MTGPermanent perm, LocationType to)
        {
            permanent = perm;
            toLocation = to;
        }

        public new void DoAction(GameState game)
        {
            var controller = permanent.Controller;

            // パーマネントの除外
            controller.Permanents.Remove(permanent);

            // スコアの減算
            var score = GetScore(controller) - permanent.Score;
            SetScore(controller, score);
        }

        public new void UndoAction(GameState game)
        {

        }
    }
}
