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
    public abstract class MTGEvent
    {
        public MTGPlayer Player { get; private set; }
        public List<MTGChoice> Choices { get; private set; }
        public MTGTarget Target { get; private set; }
        public MTGEventAction Action { get; private set; }

        public void Execute(GameState game, MTGChoiceResults choiceResults)
        {
            Action.ExecuteEvent(game, this);
        }
    }
    
    interface MTGCost
    {
    }
    
    public class MTGDrawEvent : MTGEvent
    {
    }
}