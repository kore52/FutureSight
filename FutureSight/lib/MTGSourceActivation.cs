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
        
        public IEnumerable<MTGEvent> CostEvent { get { return Activation.GetCostEvent(source); } }
        public MTGEvent Event { get { return Activation.GetEvent(source); } }
    }
}