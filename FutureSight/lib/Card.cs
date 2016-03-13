using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FutureSight.lib
{
	public enum Color
	{
		White = 0,
		Blue,
		Black,
		Red,
		Green,
		Colorless,
        Generic,
        Multicolor
    }

    [Flags]
	public enum PermanentType
	{
		Artifact = 0x01,
		Creature = 0x02,
		Enchantment = 0x04,
		Land = 0x08,
		Planeswalker = 0x10
	}

	[Flags]
	public enum CardType
	{
		Artifact = 0x01,
		Creature = 0x02,
		Enchantment = 0x04,
		Instant = 0x08,
		Land = 0x10,
		Planeswalker = 0x20,
		Sorcery = 0x40,
		Tribal = 0x80
	}

    [Serializable()]
    public class Card
	{
        public Card() { }
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
		public CardType CardType { get; set; }
		public string SubType { get; set; }
		public string SpecialType { get; set; }
		public int Power { get; set; }
		public int Toughness { get; set; }
		public string Effects { get; set; }

	}

	public class CardDB
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

		public Card get(int i)
        {
            try
            {
                return cardDB[i];
            } catch (Exception)
            {
                LoadCardDB();
                return cardDB[i];
            }
        }

		private Dictionary<int, Card> cardDB;

		private static CardDB instance = new CardDB();
	}

    [Serializable()]
    public class Permanent : Card
    {
        static int Sequencer = 0;
        public Permanent() { }
        public Permanent(int cardIndex)
        {
            CardIndex = cardIndex;
            ID = Sequencer;
            Sequencer++;
        }
        public PermanentType PermanentType { get; set; }
        public int CardIndex { get; set; }
        public int ID { get; set; }
    }
}
