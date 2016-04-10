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
    public class TakePriorityEvent : MTGEvent
    {
        public TakePriorityEvent(MTGPlayer player) : base(null, player, null, null, null) { }


        public void EventAction(MTGGame game, MTGEvent aEvent)
        {
            switch (game.Step)
            {
                case MTGStep.ActivePlayer:
                    game.Step = MTGStep.OtherPlayer;
                    break;
                case MTGStep.OtherPlayer:
                    game.Step = MTGStep.ActivePlayer;
                    break;
            }

        }
    }
}