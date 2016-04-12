using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public abstract class MTGCost
    {
    }

    public class MTGManaCost : MTGCost
    {
        private List<MTGManaSymbol> ManaCost;

        public MTGManaCost(List<MTGManaSymbol> manaCost)
        {
            ManaCost = manaCost;
        }

        public MTGManaCost(IEnumerable<MTGManaSymbol> mana)
        {
            ManaCost = new List<MTGManaSymbol>();
            foreach(var m in mana)
                ManaCost.Add(m);
        }
}

    public class MTGTapCost : MTGCost
    {
    }

    public class MTGBehaviorCost : MTGCost
    {
        public MTGBehaviorCost(string cost)
        { }
    }
    /*
    public class MTGPayLifeCost : MTGCost { }

    public class MTGSacrificeCost : MTGCost { }
    */
}
