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
        public string Name { get { return Definition.cardName; } set { Definition.cardName = Name; } }
        public CardType CardType { get { return Definition.cardType; } set { Definition.cardType = CardType; } }
        public MTGPlayer Owner { get; set; }

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

            Definition.subType = subType.GetSubTypeList();
            Definition.specialType = specialType.GetSpecialTypeList();
            Definition.power = power;
            Definition.toughness = toughness;
            
        }
        
        public MTGCard(
            MTGCardDefiniton definition,
            MTGPlayer owner
        )
        {
            Definition = definition;
            Owner = owner;
        }

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


    [Serializable()]
    public class MTGCardDefiniton
    {
        public string cardName;
        public string manaCost;
        public CardType cardType;
        public MTGSpecialTypeSet specialType;
        public MTGSubTypeSet subType;
        public string power;
        public string toughness;
        public List<string> ablitity;
        public Color ColorIndicator;
        public string loyalty;
        public string expansion;
        public string collectorNumber;
        public string illustrator;
        public string flavorText;
        public string illustURI;
        public string flippable;
        public string refFlippedCardName;
        public string transformable;
        public string refTransformedCardName;
        public string splittable;
        public string refSplittedCardName;

    }

}
