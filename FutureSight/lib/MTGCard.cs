using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    [Serializable()]
    public class MTGCard
    {
        public MTGCard() { }
        public MTGCard(string name, string manaCost, string cardType, string subType, string specialType, int power, int toughness, string effects)
        {
            this.Name = name;
            this.ManaCost = manaCost;
            if (cardType.Contains("Artifact")) { this.CardType |= CardType.Artifact;  }
            if (cardType.Contains("Creature")) { this.CardType |= CardType.Creature; }
            if (cardType.Contains("Enchantment")) { this.CardType |= CardType.Enchantment; }
            if (cardType.Contains("Instant")) { this.CardType |= CardType.Instant; }
            if (cardType.Contains("Land")) { this.CardType |= CardType.Land; }
            if (cardType.Contains("Planeswalker")) { this.CardType |= CardType.Planeswalker; }
            if (cardType.Contains("Sorcery")) { this.CardType |= CardType.Sorcery; }
            if (cardType.Contains("Tribal")) { this.CardType |= CardType.Tribal; }

            this.SubType = subType.GetSubTypeList();
            this.SpecialType = specialType.GetSpecialTypeList();
            this.Power = power;
            this.Toughness = toughness;
            this.Effects = effects;
        }
        
        public MTGCard(
            string              cardName,
            string              manaCost,
            CardType            cardType,
            MTGSpecialTypeSet   specialType,
            MTGSubTypeSet       subType,
            string              power,
            string              toughness,
            List<string>        ablitity,
            Color               ColorIndicator,
            string              loyalty,
            string              expansion,
            string              collectorNumber,
            string              illustrator,
            string              flavorText,
            string              illustURI,
            string              flippable,
            string              refFlippedCardName,
            string              transformable,
            string              refTransformedCardName,
            string              splittable,
            string              refSplittedCardName,
        )
        {}

        // properties
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public CardType CardType { get; set; }
        public List<MTGSpecialType> SpecialType { get; set; }
        public List<MTGSubType> SubType { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        public string Effects { get; set; }
        public string Descriptions { get; set; }

        public int ConvertedManaCost() {
            int cmc = 0;
            MatchCollection mc = Regex.Matches(ManaCost, @"\{(.+)\}");
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                switch (m.Value)
                {
                case "W":
                case "U":
                case "B":
                case "R":
                case "G":
                case "C":
                    cmc++;
                    break;
                case "X":
                case "Y":
                case "Z":
                    break;
                default:
                    cmc += int.Parse(m.Value);
                    break;
                }
            }
            return cmc;
        }
    }

    

    
}
