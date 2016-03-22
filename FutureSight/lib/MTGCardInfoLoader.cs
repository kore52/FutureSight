using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace FutureSight.lib
{
    class MTGCardInfoLoader
    {
        public static MTGCardInfoLoader GetInstance()
        {
            if (instance == null)
            {
                instance = new MTGCardInfoLoader();
            }
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

        public void ReadFromCSVFile(string fileName)
        {
            try
            {
                var fileStream = new System.IO.FileStream("CardInfo.csv", FileMode.Open);
                var streamReader = new System.IO.StreamReader(fileStream);
                var csvReader = new CsvReader(streamReader);
                csvReader.Configuration.HasHeaderRecord = true; // Default Value.
                csvReader.Configuration.RegisterClassMap<CardInfoFields>(); 
                while (csvReader.Read())
                {
                    var rec = csvReader.GetRecord<CardInfoFields>();
                    int hash = MurMurHash3.Hash(rec.CardName + rec.Expansion + rec.CollectorNumber);
                    database.Add(hash, new MTGCard());
                }
            }
            catch (System.Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e.Message);
#endif
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

        private Dictionary<int, MTGCard> database;
        private Dictionary<int, MTGCard> cardDB;

        private static MTGCardInfoLoader instance;
    }

    public sealed class CardInfoFields : CsvClassMap<CardInfoFields>
    {
        public CardInfoFields()
        {
            Map(m => m.CardName);
            Map(m => m.ManaCost);
            Map(m => m.CardType);
            Map(m => m.SpecialType);
            Map(m => m.SubType);
            Map(m => m.Power);
            Map(m => m.Toughness);
            Map(m => m.Ability);
            Map(m => m.ColorIndicator);
            Map(m => m.Loyalty);
            Map(m => m.Expansion);
            Map(m => m.CollectorNumber);
            Map(m => m.Illustrator);
            Map(m => m.FlavorText);
            Map(m => m.IllustURI);
            Map(m => m.Flippable);
            Map(m => m.RefFlip);
            Map(m => m.Transformable);
            Map(m => m.RefTransform);
            Map(m => m.Splittable);
            Map(m => m.RefSplit);
        }

        public string CardName { get; set; }
        public string ManaCost { get; set; }
        public string CardType { get; set; }
        public string SpecialType { get; set; }
        public string SubType { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Ability { get; set; }
        public string ColorIndicator { get; set; }
        public string Loyalty { get; set; }
        public string Expansion { get; set; }
        public string CollectorNumber { get; set; }
        public string Illustrator { get; set; }
        public string FlavorText { get; set; }
        public string IllustURI { get; set; }
        public string Flippable { get; set; }
        public string RefFlip { get; set; }
        public string Transformable { get; set; }
        public string RefTransform { get; set; }
        public string Splittable { get; set; }
        public string RefSplit { get; set; }
    }
}
