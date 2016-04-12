using System.Collections.Generic;

namespace FutureSight.lib
{
    public class MTGManaSymbolList
    {
        private List<MTGManaSymbol> ManaList { get; set; }

        public MTGManaSymbolList(IEnumerable<MTGManaSymbol> mana)
        {
            ManaList = new List<MTGManaSymbol>();
            foreach(var m in mana)
                ManaList.Add(m);
        }
    }
}