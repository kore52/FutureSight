using System;
using System.Collections;
using System.Collections.Generic;

namespace FutureSight.lib
{
    public class MTGSourceActivation : MTGSource
    {
        public MTGSource Source { get; private set; }
        public MTGActivation Activation { get; private set; }
        
        public MTGSourceActivation(MTGSource source, MTGActivation activation)
        {
            Source = source;
            Activation = activation;
        }
    }
    
    public class MTGPlayChoiceResult
    {
        public MTGSourceActivation SourceActivation { get; private set; }

        public MTGPlayChoiceResult(MTGSourceActivation aSourceActivation)
        {
            SourceActivation = aSourceActivation;
        }

        public static MTGPlayChoiceResult Pass = new MTGPlayChoiceResult(null);
        public static MTGPlayChoiceResult Skip = new MTGPlayChoiceResult(null);
    }
}