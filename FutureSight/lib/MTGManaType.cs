using System;

namespace FutureSight.lib
{
    public class MTGManaType
    {
        public ManaSymbol Type { get; private set; }
        public MTGManaType(string letter)
        {
            Type = ChooseSymbol(letter);
        }
        
        private ManaSymbol ChooseSymbol(string letter)
        {
            switch (letter)
            {
            case "W":
            case "w": return ManaSymbol.White;
            case "U":
            case "u": return ManaSymbol.Blue;
            case "B":
            case "b": return ManaSymbol.Black;
            case "R":
            case "r": return ManaSymbol.Red;
            case "G":
            case "g": return ManaSymbol.Green;
            case "X":
            case "x": return ManaSymbol.GenericX;
            case "1": return ManaSymbol.Generic1;
            case "2": return ManaSymbol.Generic2;
            case "3": return ManaSymbol.Generic3;
            case "4": return ManaSymbol.Generic4;
            case "5": return ManaSymbol.Generic5;
            case "6": return ManaSymbol.Generic6;
            case "7": return ManaSymbol.Generic7;
            case "8": return ManaSymbol.Generic8;
            case "9": return ManaSymbol.Generic9;
            case "10": return ManaSymbol.Generic10;
            case "11": return ManaSymbol.Generic11;
            case "12": return ManaSymbol.Generic12;
            case "13": return ManaSymbol.Generic13;
            case "14": return ManaSymbol.Generic14;
            case "15": return ManaSymbol.Generic15;
            default: throw new Exception("Invalid Mana Symbol");
            }
        }
    }
}