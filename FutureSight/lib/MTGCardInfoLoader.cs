using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    class MTGCardInfoLoader
    {
        public static MTGCardInfoLoader GetInstance()
        {
            return instance;
        }

        public void LoadCardDB()
        {
            cardDB = new Dictionary<int, MTGCard>()
            {
                { 1, new MTGCard("Plains", "", "Land", "Basic", "Plains", 0, 0, "{T}: add {W}") },
                { 2, new MTGCard("Island", "", "Land", "Basic", "Island", 0, 0, "{T}: add {U}") },
                { 3, new MTGCard("Swamp", "", "Land", "Basic", "Swamp", 0, 0, "{T}: add {B}") },
                { 4, new MTGCard("Mountain", "", "Land", "Basic", "Mountain", 0, 0, "{T}: add {R}") },
                { 5, new MTGCard("Forest", "", "Land", "Basic", "Forest", 0, 0, "{T}: add {G}") },
                { 6, new MTGCard("Wondering Ones", "{U}", "Creature", "", "Spirit", 1, 1, "" ) }
            };
        }

        public void ReadFromFile(string DBFileName)
        {
            // format
            // Card Name
            // Mana Cost
            // Card Type
            // Special Type
            // Sub Type
            // Power
            // Toughness
            // Ability
            // Color Indicator
            // Loyalty
            try
            {
                using (var stream = new System.IO.StreamReader(DBFileName))
                {
                    while (!stream.EndOfStream)
                    {
                    }
                }
            }
            catch (System.Exception e)
            {
            }
        }

        public MTGCard get(int i)
        {
            try
            {
                return cardDB[i];
            }
            catch (Exception)
            {
                LoadCardDB();
                return cardDB[i];
            }
        }

        private Dictionary<int, MTGCard> cardDB;

        private static CardDB instance = new CardDB();
    }
}
