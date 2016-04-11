using System;
using System.Collections;
using System.Collections.Generic;

namespace FutureSight.lib
{
    public class MTGStackItem : MTGObject
    {
        MTGSource Source;
        MTGPlayer Controller;
        MTGTarget Target;
        MTGEvent Event;
        MTGActivation Activation;
        MTGChoiceResults ChoiceResult;
        
        public MTGStackItem(MTGSource source, MTGPlayer controller, MTGTarget target, MTGEvent aEvent, MTGActivation activation)
        {
            Source = source;
            Controller = controller;
            Target = target;
            Event = aEvent;
            Activation = activation;
        }
        
        public void Resolve(MTGGame game)
        {
            game.ExecuteEvent(Event, ChoiceResult);
        }
    }
}