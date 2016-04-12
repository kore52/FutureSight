using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sprache;

namespace FutureSight.lib
{
    public class MTGRuleTextParser
    {
        public static readonly Parser<char> Colon = Parse.Char(':'); // 起動型能力の区切り

        public static readonly Parser<char> Comma = Parse.Char(','); // コストの区切り

        public static readonly Parser<string> TapSymbol = Parse.String("{T}").Text();

        public static readonly Parser<MTGManaSymbol> WhiteManaSymbol =
            from start in Parse.Char('{')
            from white in Parse.String("W").Text()
            from end in Parse.Char('}')
            select new MTGManaSymbol(white);

        public static readonly Parser<MTGManaSymbol> BlueManaSymbol =
            from start in Parse.Char('{')
            from blue in Parse.String("U").Text()
            from end in Parse.Char('}')
            select new MTGManaSymbol(blue);

        public static readonly Parser<MTGManaSymbol> BlackManaSymbol =
            from start in Parse.Char('{')
            from black in Parse.String("B").Text()
            from end in Parse.Char('}')
            select new MTGManaSymbol(black);

        public static readonly Parser<MTGManaSymbol> RedManaSymbol =
            from start in Parse.Char('{')
            from red in Parse.String("R").Text()
            from end in Parse.Char('}')
            select new MTGManaSymbol(red);

        public static readonly Parser<MTGManaSymbol> GreenManaSymbol =
            from start in Parse.Char('{')
            from green in Parse.String("G").Text()
            from end in Parse.Char('}')
            select new MTGManaSymbol(green);

        public static readonly Parser<MTGManaSymbol> ColorlessManaSymbol =
            from start in Parse.Char('{')
            from colorless in Parse.String("C").Text()
            from end in Parse.Char('}')
            select new MTGManaSymbol(colorless);

        public static readonly Parser<MTGManaSymbol> GenericManaSymbol =
            from start in Parse.Char('{')
            from generic in Parse.Number.Text()
            from end in Parse.Char('}')
            select new MTGManaSymbol(generic);

        public static readonly Parser<MTGManaSymbol> XManaSymbol =
            from start in Parse.Char('{')
            from x in Parse.String("X").Text()
            from end in Parse.Char('}')
            select new MTGManaSymbol(x);

        public static readonly Parser<MTGManaSymbol> ManaSymbolGrammer =
            from mana in WhiteManaSymbol
                .Or(BlueManaSymbol)
                .Or(BlackManaSymbol)
                .Or(RedManaSymbol)
                .Or(GreenManaSymbol)
                .Or(GenericManaSymbol)
                .Or(XManaSymbol)
            select mana;

        public static readonly Parser<List<MTGCost>> Cost =
            from mana in ManaSymbolGrammer.Many()
            from tap in TapSymbol
            from behavior in Parse.Letter.Many()
            select new List<MTGCost>() { new MTGManaCost(mana), new MTGTapCost(), new MTGBehaviorCost(string.Concat(behavior)) };


        public static readonly Parser<MTGActivation> AddManaActivationGrammer =
            from costList in Cost
            from _a in Parse.String("Add").Token().Text()
            from mana in ManaSymbolGrammer.Many()
            from _b in Parse.String("to your mana pool.").Token().Text()
            select new AddManaActivation(costList, AddManaType.Controller, new MTGManaSymbolList(mana));

        private static MTGRuleTextParser instance;
        
        
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
            var result = ActivationAbility.Parse(activation);
            return result;
        }
        
        private MTGActivation ParseActivateAbility(MatchCollection matches)
        {
            return new MTGActivation();
        }
        
        public static MTGRuleTextParser GetInstance()
        {
            if (instance == null) instance = new MTGRuleTextParser();
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