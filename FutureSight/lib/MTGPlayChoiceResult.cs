using System;
using System.Collections;
using System.Collections.Generic;

namespace FutureSight.lib
{
    public class MTGSourceActivation { }
    public class MTGPlayChoiceResult
    {
        private MTGSourceActivation sourceActivation;

        public MTGPlayChoiceResult(object aSourceActivation)
        {
            sourceActivation = aSourceActivation;
        }

        public static MTGPlayChoiceResult Pass = new MTGPlayChoiceResult(null);
        public static MTGPlayChoiceResult Skip = new MTGPlayChoiceResult(null);
    }
}