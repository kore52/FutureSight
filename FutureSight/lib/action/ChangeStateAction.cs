using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib.action
{
    public class ChangeStateAction : MTGAction
    {
        private MTGPermanent Permanent;
        private MTGPermanentState State;
        private bool Set;
        private bool Changed;

        public ChangeStateAction(MTGPermanent permanent, MTGPermanentState state, bool isSet)
        {
            Permanent = permanent;
            State = state;
            Set = isSet;
        }

        // アクションを行う
        public override void DoAction(GameState game)
        {
            Changed = Permanent.HasState(State) != Set;
            if (Changed)
            {
                if (Set)
                {
                    Permanent.State.Add(State);
                }
                else
                {
                    Permanent.State.Remove(State);
                }
                game.SetStateCheckRequired();
            }
        }

        // アクションを戻す
        public override void UndoAction(GameState game)
        {
            if (Changed)
            {
                Permanent.State.Remove(State);
            }
            else
            {
                Permanent.State.Add(State);
            }
        }
    }
}
