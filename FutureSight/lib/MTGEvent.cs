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
    public class MTGEvent
    {
        public static readonly object[] NO_CHOICE_RESULTS = new object[0];

        public MTGPlayer Player { get; private set; }
        public MTGChoice Choice { get; private set; }
        public MTGTarget Target { get; private set; }
        public MTGAction Action { get; private set; }
    }
    
    interface MTGCost
    {
    }
    
    public class MTGDrawEvent : MTGEvent
    {
    }
    
    
    public class MTGChoice
    {
        public MTGChoice() {}
    }
    
    public class MTGCardChoice : MTGChoice
    {
        public List<MTGCard> GetResults(GameState state, MTGEvent aEvent)
    }
}