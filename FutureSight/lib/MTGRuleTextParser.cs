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

        public static readonly Parser<string> TapSymbol = Parse.String("{T}").Token().Text();

        public static readonly Parser<MTGManaType> WhiteManaSymbol =
            from start in Parse.Char('{')
            from white in Parse.String("W").Text()
            from end in Parse.Char('}')
            select new MTGManaType(white);

        public static readonly Parser<MTGManaType> BlueManaSymbol =
            from start in Parse.Char('{')
            from blue in Parse.String("U").Text()
            from end in Parse.Char('}')
            select new MTGManaType(blue);

        public static readonly Parser<MTGManaType> BlackManaSymbol =
            from start in Parse.Char('{')
            from black in Parse.String("B").Text()
            from end in Parse.Char('}')
            select new MTGManaType(black);

        public static readonly Parser<MTGManaType> RedManaSymbol =
            from start in Parse.Char('{')
            from red in Parse.String("R").Text()
            from end in Parse.Char('}')
            select new MTGManaType(red);

        public static readonly Parser<MTGManaType> GreenManaSymbol =
            from start in Parse.Char('{')
            from green in Parse.String("G").Text()
            from end in Parse.Char('}')
            select new MTGManaType(green);

        public static readonly Parser<MTGManaType> ColorlessManaSymbol =
            from start in Parse.Char('{')
            from colorless in Parse.String("C").Text()
            from end in Parse.Char('}')
            select new MTGManaType(colorless);

        public static readonly Parser<MTGManaType> GenericManaSymbol =
            from start in Parse.Char('{')
            from generic in Parse.Number.Text()
            from end in Parse.Char('}')
            select new MTGManaType(generic);

        public static readonly Parser<MTGManaType> XManaSymbol =
            from start in Parse.Char('{')
            from x in Parse.String("X").Text()
            from end in Parse.Char('}')
            select new MTGManaType(x);

        public static readonly Parser<MTGManaType> ManaSymbolGrammer =
            from mana in WhiteManaSymbol
                .Or(BlueManaSymbol)
                .Or(BlackManaSymbol)
                .Or(RedManaSymbol)
                .Or(GreenManaSymbol)
                .Or(GenericManaSymbol)
                .Or(XManaSymbol)
            select mana;

        // {T} : Add {X} to your mana pool.
        public static readonly Parser<TapManaActivation> TapManaActivationParser = 
            from cost in TapSymbol
            from _ in Colon
            from _add in Parse.String("Add").Token().Text()
            from mana in ManaSymbolGrammer.Many()
            from _tymp in Parse.String("to your mana pool.").Token().Text()
            select new TapManaActivation(TapManaActivation.Create(cost, new List<MTGManaType>(mana.ToList())));

        public static readonly object[] ParseList =
            {
                TapManaActivationParser
            };
        
        public List<MTGActivation> ParseActivations(string activationString)
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
            foreach (var parser in MTGRuleTextParser.ParseList)
            {
                if (parser is Parser<MTGManaActivation>)
                {
                    var castParser = (Parser<MTGManaActivation>)parser;
                    var obj = castParser.Parse(ab);
                    if (obj is MTGManaActivation)
                    {
                        Console.WriteLine("read ManaActivation.");
                        ManaActivations.Add(obj);
                        break;
                    }
                }
            }
            return result;
        }
        
        private static MTGRuleTextParser instance;
        public static MTGRuleTextParser GetInstance()
        {
            if (instance == null) instance = new MTGRuleTextParser();
            return instance;
        }
    }
}