﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    /// イベントに対するアクション定義の基底クラス
    public abstract class MTGAction
    {
        private MTGPlayer scorePlayer;
        private int score;

        // アクションを行う
        public abstract void DoAction(MTGGame game);

        // アクションを戻す
        public abstract void UndoAction(MTGGame game);

        // アクションスコアをセット
        public void SetScore(MTGPlayer scorePlayer, int score)
        {
            this.scorePlayer = scorePlayer;
            this.score = score;
        }

        // アクションに対するスコアを取得
        // アクションを行うプレイヤーが評価するプレイヤーと同じであればプラス、対戦相手であればマイナス
        public int GetScore(MTGPlayer player)
            => (player.ID == scorePlayer.ID) ? score : -score;
        
        public MTGPlayer GetScorePlayer()
            => scorePlayer;
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

        public override void DoAction(MTGGame game)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(player.Hand.Contains(card));
#endif
            permanent = new MTGPermanent(card);
            player.Permanents.Add(permanent);
            player.Hand.Remove(card);

            // スコア計算
            SetScore(player, permanent.Score);
        }

        public override void UndoAction(MTGGame game)
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

        public override void DoAction(MTGGame game)
        {
            firstEvent = game.Events.First;
            game.ExecuteEvent(firstEvent, choiceResults);
        }

        public override void UndoAction(MTGGame game)
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

        public override void DoAction(MTGGame game)
        {
            var controller = permanent.Controller;

            // パーマネントの除外
            controller.Permanents.Remove(permanent);

            // スコア計算
            var score = GetScore(controller) - permanent.Score;
            SetScore(controller, score);
        }

        public override void UndoAction(MTGGame game)
        {

        }
    }
}
