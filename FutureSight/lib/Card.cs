using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    [Flags]
    enum Color
    {
        White,
        Blue,
        Black,
        Red,
        Green,
        Multicolor,
        Colorless
    }

    [Flags]
    enum PermanentType
    {
        Artifact,
        Creature,
        Enchantment,
        Land,
        Planeswalker
    }

    [Flags]
    enum CardType
    {
        Artifact,
        Creature,
        Enchantment,
        Instant,
        Land,
        Planeswalker,
        Sorcery,
        Tribal
    }
   
    class Card
    {
        public Card(string name, string manaCost, string cardType, string subType, string specialType, int power, int toughness, string effects)
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

            this.SubType = subType;
            this.SpecialType = specialType;
            this.Power = power;
            this.Toughness = toughness;
            this.Effects = effects;
        }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public CardType CardType { get; set; }
        public string SubType { get; set; }
        public string SpecialType { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        public string Effects { get; set; }

    }

    class CardDB
    {
        public static CardDB GetInstance()
        {
            return instance;
        }

        public void LoadCardDB()
        {
            cardDB = new Dictionary<int, Card>()
            {
                { 1, new Card("Plains", "", "Land", "Basic", "Plains", 0, 0, "{T}: add {W}") },
                { 2, new Card("Island", "", "Land", "Basic", "Island", 0, 0, "{T}: add {U}") },
                { 3, new Card("Swamp", "", "Land", "Basic", "Swamp", 0, 0, "{T}: add {B}") },
                { 4, new Card("Mountain", "", "Land", "Basic", "Mountain", 0, 0, "{T}: add {R}") },
                { 5, new Card("Forest", "", "Land", "Basic", "Forest", 0, 0, "{T}: add {G}") },
                { 6, new Card("Wondering Ones", "{U}", "Creature", "", "Spirit", 1, 1, "" ) }
            };
        }

        public Card get(int i) { return cardDB[i]; }

        private Dictionary<int, Card> cardDB;

        private static CardDB instance = new CardDB();
    }
}
