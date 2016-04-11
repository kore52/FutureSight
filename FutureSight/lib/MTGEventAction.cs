using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class MTGEventAction
    {
        public Action<MTGGame, MTGEvent> ExecuteEvent;
        
        public MTGEventAction(Action<MTGGame, MTGEvent> action)
        {
            ExecuteEvent = action;
        }
        
        // 何もしないアクション
        public static MTGEventAction None;
        static MTGEventAction()
        {
            None = new MTGEventAction((MTGGame game, MTGEvent aEvent) => {});
        }
    }
}
