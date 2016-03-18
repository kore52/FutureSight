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
    public class MTGTarget
    {
        public MTGTarget(string type)
        {
            targetType = type;
        }
        
        public Object[] GetLegalTargets(Object[] targets)
        {
            return new Object[];
        }

        public string TargetType { get { return targetType; } };

        protected string targetType;
    }
    
    public class MTGNoneTarget : MTGTarget
    {
        public MTGNoneTarget() : base("none") {}
        
        public Object[] new GetLegalTargets(Object[] targets)
        {
            return new Object[];
        }
    }
    
    public class MTGPlayerTarget : MTGTarget
    {
        public MTGPlayerTarget() : base("MTGPlayerTarget") {}
        
        public Object[] new GetLegalTargets(Object[] targets)
        {
            var results = new List<PlayerState>();
            if (typeof(targets).Name != TargetType) { return new Object[]; }
            foreach(var player in targets)
            {
                results.Add(player);
            }
            return results;
        }
    }
}