using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sprache;

namespace FutureSight.lib
{
    public class MTGActivationBuilder
    {
        public static readonly Parser<char> Colon = Parse.Char(':'); // 起動型能力の区切り
        public static readonly Parser<char> Comma = Parse.Char(','); // コストの区切り
        public static readonly Parser<string> TapSymbol = Parse.String("{T}").Text();
        public static readonly Parser<string> WhiteManaSymbol =
            from start in Parse.Char('{')
            from white in Parse.String("W").Text()
            from end in Parse.Char('}')
            select white;
        public static readonly Parser<string> BlueManaSymbol =
            from start in Parse.Char('{')
            from blue in Parse.String("U").Text()
            from end in Parse.Char('}')
            select blue;
        public static readonly Parser<string> BlackManaSymbol =
            from start in Parse.Char('{')
            from black in Parse.String("B").Text()
            from end in Parse.Char('}')
            select black;
        public static readonly Parser<string> RedManaSymbol =
            from start in Parse.Char('{')
            from red in Parse.String("R").Text()
            from end in Parse.Char('}')
            select red;
        public static readonly Parser<string> GreenManaSymbol =
            from start in Parse.Char('{')
            from green in Parse.String("G").Text()
            from end in Parse.Char('}')
            select green;
        public static readonly Parser<string> ManaSymbol =
            from mana in WhiteManaSymbol.Text()
            .Or(BlueManaSymbol.Text())
            .Or(BlackManaSymbol.Text())
            .Or(RedManaSymbol.Text())
            .Or(GreenManaSymbol.Text())
            select mana;
        public static readonly Parser<string> AddToYourManaPool =
            from _1 in Parse.String("Add").Token().Text()
            from mana in ManaSymbol
            from _2 in Parse.String("to your mana pool.").Token().Text()
            select mana;
        
        public static readonly Parser<string> Cost =
        from tapsymbol in TapSymbol.Many().Token().Text()
        from c in Comma.Many().Token().Text()
        from manasymbol in ManaSymbol.Many().Token().Text()
        from c in Comma.Many().Token().Text()
        from 
        
        public static readonly Parser<MTGActivation> ActivationAbility =
        from cost in TapSymbol.Token().Text()
        from 


        private static MTGActivationBuilder instance;
        
        
        public List<MTGActivation> Build(string activationString)
        {
            var actsResult = new List<MTGActivation>();
            var activations = activationString.Split('|');
            foreach(var act in activations)
            {
                actsResult.Add(ParserActivation(act));
            }
            return actsResult;
        }
        
        private MTGActivation ParserActivation(string activation)
        {
            
            //var activation = 
            return new MTGActivaton();
        }
        
        private MTGActivation ParseActivateAbility(MatchCollection matches)
        {
            return new MTGActivation();
        }
        
        public static MTGActivationBuilder GetInstance()
        {
            if (instance == null) instance = new MTGActivationBuilder();
            return instance;
        }
    }
    
    
    public class MTGManaSymbol
    {
        public ManaSymbol Type { get; private set; }
        public MTGManaSymbol(string letter)
        {
            switch (letter)
            {
            case "W":
            case "w":
                Type = ManaSymbol.White;
                break;
            case "U":
            case "u":
                Type = ManaSymbol.Blue;
                break;
            case "B":
            case "b":
                Type = ManaSymbol.Black;
                break;
            case "R":
            case "r":
                Type = ManaSymbol.Red;
                break;
            case "G":
            case "g":
                Type = ManaSymbol.Green;
                break;
            default:
                throw new Exception("Invalid Mana Symbol");
            }
        }
    }
}