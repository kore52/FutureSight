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
    public abstract class MTGTarget
    {
        public MTGTarget() { }
        public MTGTarget(string type)
        {
            targetType = type;
        }

        public List<MTGTarget> GetLegalTargets(MTGGame game)
        {
            return new List<MTGTarget>();
        }

        public string TargetType { get { return targetType; } }

        protected string targetType;
    }
    
    public class MTGNoneTarget : MTGTarget
    {
        public MTGNoneTarget() : base("none") {}
    }
    
    public class MTGPlayerTarget : MTGTarget
    {
        public MTGPlayerTarget() : base("MTGPlayerTarget") {}

        public new List<MTGTarget> GetLegalTargets(MTGGame game)
        {
            var results = new List<MTGTarget>();
            foreach(var player in game.Players)
            {
                results.Add(player);
            }
            return results;
        }
    }

    public class MTGPermanentTarget : MTGTarget
    {
        public MTGPermanentTarget() : base("MTGPlayerTarget") { }

    }
}