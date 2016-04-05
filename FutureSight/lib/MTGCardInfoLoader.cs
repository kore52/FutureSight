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
        public Dictionary<int, MTGCardDefinition> Database { get; }
        private Dictionary<int, MTGCard> cardDB;
        private static MTGCardInfoLoader instance;

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
            cardDB = new Dictionary<string, MTGCard>()
            {
                { "Plains", new MTGCard("Plains", "", "Land", "Basic", "Plains", 0, 0, "{T}: add {W}") },
                { "Island", new MTGCard("Island", "", "Land", "Basic", "Island", 0, 0, "{T}: add {U}") },
                { "Swamp", new MTGCard("Swamp", "", "Land", "Basic", "Swamp", 0, 0, "{T}: add {B}") },
                { "Mountain", new MTGCard("Mountain", "", "Land", "Basic", "Mountain", 0, 0, "{T}: add {R}") },
                { "Forest", new MTGCard("Forest", "", "Land", "Basic", "Forest", 0, 0, "{T}: add {G}") },
                { "Wondering Ones", new MTGCard("Wondering Ones", "{U}", "Creature", "", "Spirit", 1, 1, "" ) }
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
                csvReader.Configuration.RegisterClassMap<CardDefinitionMap>(); 
                while (csvReader.Read())
                {
                    var rec = csvReader.GetRecord<MTGCardTextField>();
                    Database.Add(
                        rec.CardName,
                        new MTGCardDefiniton(
                            rec.CardName,
                            rec.ManaCost,
                            MTGCardDefinition.GetCardType(rec.CardType),
                            new MTGSpecialTypeSet(rec.SpecialType),
                            new MTGSubTypeSet(rec.SubType),
                            rec.Power,
                            rec.Toughness,
                            new List<string>(rec.Ability.Split('|')),
                            MTGCardDefinition.GetCardType(rec.CardType),
                            rec.Loyalty,
                            rec.Expansion,
                            rec.CollectorNumber,
                            rec.Illustrator,
                            rec.FlavorText,
                            rec.IllustURI,
                            rec.Flippable,
                            rec.RefFlippedCardName,
                            rec.Transformable,
                            rec.RefTransformedCardName,
                            rec.Splittable,
                            rec.RefSplittedCardName
                        )
                    );
                }
            }
            catch (System.Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e.Message);
#endif
            }
        }
    }

    [Serializable()]
    public class MTGCardTextField
    {
        public string CardName;
        public string ManaCost;
        public string CardType;
        public string SpecialType;
        public string SubType;
        public string Power;
        public string Toughness;
        public string Ability;
        public string ColorIndicator;
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
    
    public sealed class CardDefinitionMap : CsvClassMap<MTGCardTextField>
    {
        public CardDefinitionMap()
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
            Map(m => m.RefFlippedCardName);
            Map(m => m.Transformable);
            Map(m => m.RefTransformedCardName);
            Map(m => m.Splittable);
            Map(m => m.RefSplittedCardName);
        }
    }
}
