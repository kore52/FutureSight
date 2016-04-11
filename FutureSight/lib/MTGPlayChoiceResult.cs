using System;
using System.Collections;
using System.Collections.Generic;

namespace FutureSight.lib
{
    public class MTGSourceActivation : MTGActivation { }
    public class MTGPlayChoiceResult
    {
        private MTGSourceActivation sourceActivation;

        public MTGPlayChoiceResult(MTGSourceActivation aSourceActivation)
        {
            sourceActivation = aSourceActivation;
        }

        public static MTGPlayChoiceResult Pass = new MTGPlayChoiceResult(null);
        public static MTGPlayChoiceResult Skip = new MTGPlayChoiceResult(null);
    }
}