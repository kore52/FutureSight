using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class LoseGameAction : MTGAction
    {
        public enum LoseGameReason
        {
            Life,
            Poison,
            Draw,
            Effects
        }
        
        private MTGPlayer player;
        private LoseGameReason reason;

        public LoseGameAction(MTGPlayer player, LoseGameReason reason)
        {
            this.player = player;
            this.reason = reason;
        }
        
        public LoseGameAction(MTGPlayer player) : this(player, LoseGameReason.Life) { }

        // アクションを行う
        public override void DoAction(MTGGame game)
        {
            player.State.Add(MTGPlayerState.LoseGame);
            game.Log(player, reason.ToString());
        }

        // アクションを戻す
        public override void UndoAction(MTGGame game)
        {
            player.State.Remove(MTGPlayerState.LoseGame);
        }
    }
}