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
        public MTGPlayer Owner { get; set; }
        public MTGCardDefinition Definition { get; set; }
        public string Name { get { return Definition.CardName; } set { Definition.CardName = Name; } }
        public CardType CardType { get { return Definition.CardType; } set { Definition.CardType = CardType; } }
        
        public MTGCard() { }
        public MTGCard(string name, string manaCost, string cardType, string subType, string specialType, int power, int toughness, string effects)
        {
            this.Name = name;
            
            Definition.CardType = CardDefinition.GetCardType(cardType);
            Definition.SubType = new MTGSubTypeSet(subType.GetSubTypeList());
            Definition.SpecialType = new MTGSpecialTypeSet(specialType.GetSpecialTypeList());
            Definition.Power = power.ToString();
            Definition.Toughness = toughness.ToString();
            
        }
        
        public MTGCard(MTGCardDefinition def, MTGPlayer owner)
        {
            Definition = def;
            Owner = owner;
        }
        
    }


    [Serializable()]
    public class MTGCardDefinition
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

        public MTGCardDefinition()
        {
            Ability = new List<string>();
            SpecialType = new MTGSpecialTypeSet();
            SubType = new MTGSubTypeSet();
        }
        
        public MTGCardDefinition(
            string cardName,
            string manaCost,
            CardType cardType,
            MTGSpecialTypeSet specialType,
            MTGSubTypeSet subType,
            string power,
            string toughness,
            List<string> ability,
            Color colorIndicator,
            string loyalty,
            string expansion,
            string collectorNumber,
            string illustrator,
            string flavorText,
            string illustURI,
            string flippable,
            string refFlippedCardName,
            string transformable,
            string refTransformedCardName,
            string splittable,
            string refSplittedCardName)
        {
            CardName = cardName;
            ManaCost = manaCost;
            CardType = cardType;
            SpecialType = specialType;
            SubType = subType;
            Power = power;
            Toughness = toughness;
            Ability = ability;
            ColorIndicator = colorIndicator;
            Loyalty = loyalty;
            Expansion = expansion;
            CollectorNumber = collectorNumber;
            Illustrator = illustrator;
            FlavorText = flavorText;
            IllustURI = illustURI;
            Flippable = flippable;
            RefFlippedCardName = refFlippedCardName;
            Transformable = transformable;
            RefTransformedCardName = refTransformedCardName;
            Splittable = splittable;
            RefSplittedCardName = refSplittedCardName;
        }
        
        public static CardType GetCardType(string cardType)
        {
            CardType result;
            if (cardType.Contains("Artifact")) { result |= CardType.Artifact;  }
            if (cardType.Contains("Creature")) { result |= CardType.Creature; }
            if (cardType.Contains("Enchantment")) { result |= CardType.Enchantment; }
            if (cardType.Contains("Instant")) { result |= CardType.Instant; }
            if (cardType.Contains("Land")) { result |= CardType.Land; }
            if (cardType.Contains("Planeswalker")) { result |= CardType.Planeswalker; }
            if (cardType.Contains("Sorcery")) { result |= CardType.Sorcery; }
            if (cardType.Contains("Tribal")) { result |= CardType.Tribal; }
            return result;
        }
        
        public static Color GetColorType(string color)
        {
            Color result;
            if (color.Contains("W")) { result |= Color.White; }
            if (color.Contains("U")) { result |= Color.Blue; }
            if (color.Contains("B")) { result |= Color.Black; }
            if (color.Contains("R")) { result |= Color.Red; }
            if (color.Contains("G")) { result |= Color.Green; }
            if (color == "") { result = Color.Colorless; }
            return result;
        }
    }

}
