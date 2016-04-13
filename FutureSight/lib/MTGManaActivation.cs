using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class MTGManaActivation
    {
        private string cost;
        private List<MTGManaType> manaType;
        
        public MTGManaActivation(string cost, List<MTGManaType> manaType)
        {
            this.cost = cost;
            this.manaType = manaType;
        }
        
        public static MTGManaActivation Create(string cost, List<MTGManaType> manaType)
        {
            return new MTGManaActivation(cost, manaType);
        }
        
        public IEnumerable<MTGEvent> GetCostEvent(MTGPermanent permanent)
        {
            yield return new TapEvent(permanent);
        }
    }
}