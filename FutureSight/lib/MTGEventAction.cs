using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class MTGEventAction
    {
        public void ExecuteEvent(MTGGame game, MTGEvent mtgevent) { }

        public static MTGEventAction NONE;

        static MTGEventAction()
        {
            NONE = new MTGEventAction();
        }
    }
}
