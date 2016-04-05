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
        public Dictionary<string, MTGCardDefinition> Database { get; }
        private Dictionary<string, MTGCard> cardDB;
        private static MTGCardInfoLoader instance;

        public static MTGCardInfoLoader GetInstance()
        {
            if (instance == null) instance = new MTGCardInfoLoader();
            return instance;
        }

        public MTGCardInfoLoader()
        {
            Database = new Dictionary<string, MTGCardDefinition>();
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
                using (var fileStream = new System.IO.FileStream(".\\resources\\CardInfo.csv", FileMode.Open))
                {
                    var streamReader = new System.IO.StreamReader(fileStream);
                    var csvReader = new CsvReader(streamReader);
                    csvReader.Configuration.HasHeaderRecord = false; // Default Value.
                    csvReader.Configuration.RegisterClassMap<CardDefinitionMap>(); 
                    while (csvReader.Read())
                    {
                        var rec = csvReader.GetRecord<MTGCardTextField>();
                        Database.Add(
                            rec.CardName,
                            new MTGCardDefinition(
                                rec.CardName,
                                rec.ManaCost,
                                MTGCardDefinition.GetCardType(rec.CardType),
                                new MTGSpecialTypeSet(rec.SpecialType),
                                new MTGSubTypeSet(rec.SubType),
                                rec.Power,
                                rec.Toughness,
                                new List<string>(rec.Ability.Split('|')),
                                MTGCardDefinition.GetColorType(rec.ColorIndicator),
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
            }
            catch (Exception e)
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
        public string RefFlippedCardName { get; set; }
        public string Transformable { get; set; }
        public string RefTransformedCardName { get; set; }
        public string Splittable { get; set; }
        public string RefSplittedCardName { get; set; }
    }
    
    public sealed class CardDefinitionMap : CsvClassMap<MTGCardTextField>
    {
        public CardDefinitionMap()
        {
            Map(m => m.CardName).Index(0);
            Map(m => m.ManaCost).Index(1);
            Map(m => m.CardType).Index(2);
            Map(m => m.SpecialType).Index(3);
            Map(m => m.SubType).Index(4);
            Map(m => m.Power).Index(5);
            Map(m => m.Toughness).Index(6);
            Map(m => m.Ability).Index(7);
            Map(m => m.ColorIndicator).Index(8);
            Map(m => m.Loyalty).Index(9);
            Map(m => m.Expansion).Index(10);
            Map(m => m.CollectorNumber).Index(11);
            Map(m => m.Illustrator).Index(12);
            Map(m => m.FlavorText).Index(13);
            Map(m => m.IllustURI).Index(14);
            Map(m => m.Flippable).Index(15);
            Map(m => m.RefFlippedCardName).Index(16);
            Map(m => m.Transformable).Index(17);
            Map(m => m.RefTransformedCardName).Index(18);
            Map(m => m.Splittable).Index(19);
            Map(m => m.RefSplittedCardName).Index(20);
        }
    }
}
