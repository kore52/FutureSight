using System;

namespace FutureSight.lib
{
    public class MTGManaType
    {
        public ManaType Type { get; private set; }
        public MTGManaType(string letter)
        {
            Type = GetType(letter);
        }
        
        private ManaType GetType(string letter)
        {
            switch (letter)
            {
            case "W":
            case "w": return ManaType.White;
            case "U":
            case "u": return ManaType.Blue;
            case "B":
            case "b": return ManaType.Black;
            case "R":
            case "r": return ManaType.Red;
            case "G":
            case "g": return ManaType.Green;
            case "X":
            case "x": return ManaType.GenericX;
            case "1": return ManaType.Generic1;
            case "2": return ManaType.Generic2;
            case "3": return ManaType.Generic3;
            case "4": return ManaType.Generic4;
            case "5": return ManaType.Generic5;
            case "6": return ManaType.Generic6;
            case "7": return ManaType.Generic7;
            case "8": return ManaType.Generic8;
            case "9": return ManaType.Generic9;
            case "10": return ManaType.Generic10;
            case "11": return ManaType.Generic11;
            case "12": return ManaType.Generic12;
            case "13": return ManaType.Generic13;
            case "14": return ManaType.Generic14;
            case "15": return ManaType.Generic15;
            default: throw new Exception("Invalid Mana Type.");
            }
        }
    }
}