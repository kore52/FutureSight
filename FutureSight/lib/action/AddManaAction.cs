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
        private List<MTGManaType> manaTypeList;

        public AddManaActivation(List<MTGCost> costList, AddManaType manaType, List<MTGManaType> manaTypeList)
        {
            this.manaType = manaType;
            this.manaTypeList = manaTypeList;
        }
    }
}