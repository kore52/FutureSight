using System;
using System.Collections;
using System.Collections.Generic;

namespace FutureSight.lib
{
    public enum AddManaType
    {
        Controller,
        ActivePlayer,
    }

    public class AddManaActivation : MTGActivation
    {
        private AddManaType manaType;
        private MTGManaSymbolList manaSymbolList;

        public AddManaActivation(List<MTGCost> costList, AddManaType manaType, MTGManaSymbolList manaSymbolList)
        {
            this.manaType = manaType;
            this.manaSymbolList = manaSymbolList;
        }
    }
}