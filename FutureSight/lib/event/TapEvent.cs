using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

namespace FutureSight.lib
{
    public class TapEvent : MTGEvent
    {
        public TapEvent(MTGPermanent permanent)
            : base(permanent, EventAction, "Tap SN.") { }
        
        public static MTGEventAction EventAction;
        
        static TapEvent()
        {
            EventAction = new MTGEventAction((MTGGame game, MTGEvent aEvent) =>
            {
                game.DoAction(new TapAction(aEvent.Permanent));
            });
        }
    }
}