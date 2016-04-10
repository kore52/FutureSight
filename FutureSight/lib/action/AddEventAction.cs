using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class AddEventAction : MTGAction
    {
        private MTGEvent aEvent;

        public AddEventAction(MTGEvent aEvent)
        {
            this.aEvent = aEvent;
        }

        // アクションを行う
        public override void DoAction(MTGGame game)
        {
            game.Events.AddLast(aEvent);
        }

        // アクションを戻す
        public override void UndoAction(MTGGame game)
        {
            game.Events.RemoveLast();
        }
    }
}