using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    [Serializable()]
    public class MTGCard : MTGSource
    {
        public MTGPlayer Owner { get; set; }
        public MTGCardDefinition Definition { get; set; }
        public string Name { get { return Definition.CardName; } set { Definition.CardName = Name; } }
        public CardType CardType { get { return Definition.CardType; } set { Definition.CardType = CardType; } }
        public List<MTGActivation> Activations { get; private set; }
        public int Score { get { return int.Parse(Definition.Score); } }
        
        public MTGCard() { }
        public MTGCard(string name, string manaCost, string cardType, string subType, string specialType, int power, int toughness, string effects)
        {
            this.Name = name;
            
            Definition.CardType = MTGCardDefinition.GetCardType(cardType);
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
        public string Score;

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
            string refSplittedCardName
        ) : this(cardName, manaCost, cardType, specialType, subType, power, toughness, ability, colorIndicator, loyalty, expansion, collectorNumber, illustrator, flavorText, illustURI, flippable, refFlippedCardName, transformable, refTransformedCardName, splittable, refSplittedCardName, "1") {}
        
        public MTGCardDefinition(
            string cardName,
            string manaCost,
            CardType cardType,
            MTGSpecialTypeSet specialType,
            MTGSubTypeSet subType,
            string power,
            string toughness,
            List<MTGActivation> ability,
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
            string refSplittedCardName,
            string score
        )
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
            Score = score;
        }
        
        public static CardType GetCardType(string cardType)
        {
            CardType result = CardType.Unknown;
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
            Color result = Color.Colorless;
            if (color.Contains("W")) { result |= Color.White; }
            if (color.Contains("U")) { result |= Color.Blue; }
            if (color.Contains("B")) { result |= Color.Black; }
            if (color.Contains("R")) { result |= Color.Red; }
            if (color.Contains("G")) { result |= Color.Green; }
            if (color == "") { result = Color.Colorless; }
            return result;
        }
    }

    public class MTGCardList : List<MTGCard>
    {
        public void AddToTop(MTGCard card)
            => Add(card);
        
        public void AddToBottom(MTGCard card)
            => Insert(0, card);
        
        public void RemoveFromTop()
            => RemoveAt(Count-1);
        
        public void RemoveFromBottom()
            => RemoveAt(0);
        
        public bool IsEmpty()
            => Count == 0;
    }
}
