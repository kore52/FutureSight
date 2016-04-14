using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public abstract class MTGManaActivation
    {
        public abstract IEnumerable<MTGEvent> GetCostEvent(MTGPermanent permanent);
    }
    
    public class TapManaActivation : MTGManaActivation
    {
        private List<MTGManaType> manaType;
        
        public TapManaActivation(List<MTGManaType> manaType)
        {
            this.manaType = manaType;
        }
        
        public static TapManaActivation Create(List<MTGManaType> manaType)
        {
            return new TapManaActivation(manaType);
        }
        
        public MTGEvent GetEvent()
        {
            return 
        }
        
        public override IEnumerable<MTGEvent> GetCostEvent(MTGPermanent permanent)
        {
            yield return new TapEvent(permanent);
        }
        /*
        public static TapManaActivation WhiteManaActivation;
        public static TapManaActivation BlueManaActivation;
        public static TapManaActivation BlackManaActivation;
        public static TapManaActivation RedManaActivation;
        public static TapManaActivation GreenManaActivation;
        
        static TapManaActivation
        {
            WhiteManaActivation = new TapManaActivation(MTGRuleTextParser.ManaSymbolGrammer("{W}"));
            BlueManaActivation = new TapManaActivation();
            BlackManaActivation = new TapManaActivation();
            RedManaActivation = new TapManaActivation();
            GreenManaActivation = new TapManaActivation();
        }*/
    }
}