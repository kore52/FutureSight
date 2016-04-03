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
        public MTGCardDefiniton Definition { get; set; }
        public string Name { get { return Definition.CardName; } set { Definition.CardName = Name; } }
        public CardType CardType { get { return Definition.CardType; } set { Definition.CardType = CardType; } }
        public MTGPlayer Owner { get; set; }

        public MTGCard() { }
        public MTGCard(string name, string manaCost, string cardType, string subType, string specialType, int power, int toughness, string effects)
        {
            this.Name = name;
            if (cardType.Contains("Artifact")) { this.CardType |= CardType.Artifact;  }
            if (cardType.Contains("Creature")) { this.CardType |= CardType.Creature; }
            if (cardType.Contains("Enchantment")) { this.CardType |= CardType.Enchantment; }
            if (cardType.Contains("Instant")) { this.CardType |= CardType.Instant; }
            if (cardType.Contains("Land")) { this.CardType |= CardType.Land; }
            if (cardType.Contains("Planeswalker")) { this.CardType |= CardType.Planeswalker; }
            if (cardType.Contains("Sorcery")) { this.CardType |= CardType.Sorcery; }
            if (cardType.Contains("Tribal")) { this.CardType |= CardType.Tribal; }

            Definition.SubType = new MTGSubTypeSet(subType.GetSubTypeList());
            Definition.SpecialType = new MTGSpecialTypeSet(specialType.GetSpecialTypeList());
            Definition.Power = power.ToString();
            Definition.Toughness = toughness.ToString();
            
        }
        
        public MTGCard(
            MTGCardDefiniton definition,
            MTGPlayer owner
        )
        {
            Definition = definition;
            Owner = owner;
        }

  /*      public int ConvertedManaCost() {
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
        }*/
    }


    [Serializable()]
    public class MTGCardDefiniton
    {
        public string CardName;
        public string ManaCost;
        public CardType CardType;
        public MTGSpecialTypeSet SpecialType;
        public MTGSubTypeSet SubType;
        public string Power;
        public string Toughness;
        public List<string> Ability;
        public Color ColorIndicator;
        public string Loyalty;
        public string Expansion;
        public string CollectorNumber;
        public string Illustrator;
        public string FlavorText;
        public string IllustURI;
        public string Flippable;
        public string RefFlippedCardName;
        public string Transformable;
        public string RefTransformedCardName;
        public string Splittable;
        public string RefSplittedCardName;

    }

}
