using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class TapAction : MTGAction
    {
        private MTGPermanent permanent;
        private bool isUntapped;

        public TapAction(MTGPermanent permanent)
        {
            this.permanent = permanent;
        }

        // アクションを行う
        public override void DoAction(MTGGame game)
        {
            isUntapped = permanent.IsUntapped();
            if (isUntapped)
            {
                permanent.AddState(MTGPermanentState.Tapped);
                game.SetStateCheckRequired();
            }
        }

        // アクションを戻す
        public override void UndoAction(MTGGame game)
        {
            if (isUntapped)
                permanent.RemoveState(MTGPermanentState.Tapped);
        }
    }
}