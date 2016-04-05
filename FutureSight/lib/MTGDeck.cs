using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FutureSight.lib
{
    public class MTGDeck
    {
        public List<MTGCardDefinition> MainDeck { get; }
        public List<MTGCardDefinition> Sideboard { get; }

        public MTGDeck()
        {
            MainDeck = new List<MTGCardDefinition>();
            Sideboard = new List<MTGCardDefinition>();
        }
    }
    
    public class DeckLoader
    {
        public static DeckLoader GetInstance()
        {
            if (instance == null) instance = new DeckLoader();
            return instance;
        }

        public MTGDeck LoadFromFile(string filename)
        {
            if (Regex.Match(filename.ToLower(), "\\.dek$").Success) return DEKFormatReader.GetInstance().Load(filename);
            else if (Regex.Match(filename.ToLower(), "\\.txt$").Success) return TXTFormatReader.GetInstance().Load(filename);
            throw new Exception("Invalid Deck File extension.");
        }

        private static DeckLoader instance;
    }
    
    interface IDeckFileFormatReader
    {
        MTGDeck Load(string filename);
    }
    
    public class DEKFormatReader : IDeckFileFormatReader
    {
        public static DEKFormatReader GetInstance()
        {
            if (instance == null) instance = new DEKFormatReader();
            return instance;
        }

        public MTGDeck Load(string filename)
        {
            MTGDeck deck = new MTGDeck();
            try
            {
                var doc = XDocument.Load(filename);
                foreach (var elem in doc.Descendants("Deck"))
                {
                    var quantity = int.Parse(elem.Attribute("Quantity").Value);
                    var match = Regex.Match(elem.Attribute("Sideboard").Value.ToLower(), "true");
                    bool isSideBoard = (match.Success) ? true : false;
                    var name = elem.Attribute("Name").Value;

                    var card = (MTGCardDefinition)Utilities.DeepCopy(MTGCardInfoLoader.GetInstance().Database[name]);
                    for ( int i = 0; i < quantity; i++ )
                    {
                        if (!isSideBoard) deck.MainDeck.Add(card);
                        else deck.Sideboard.Add(card);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Invalid Deck File format.");
            }
            
            return deck;
        }

        private static DEKFormatReader instance;
    }
    
    public class TXTFormatReader : IDeckFileFormatReader
    {
        public static TXTFormatReader GetInstance()
        {
            if (instance == null) instance = new TXTFormatReader();
            return instance;
        }

        public MTGDeck Load(string filename)
        {
            MTGDeck deck = new MTGDeck();
            try
            {
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    var tr = new StreamReader(fs);

                    string line;
                    bool isSideBoard = false;
                    while ((line = tr.ReadLine()) != null)
                    {
                        var regex = new Regex(@"^(?<qty>[0-9]+) (?<name>.+)$");
                        var matches = regex.Match(line);
                        if (matches.Success)
                        {
                            var quantity = int.Parse(matches.Groups["qty"].Value);
                            //matches.NextMatch();
                            var name = matches.Groups["name"].Value;
                            var srccard = MTGCardInfoLoader.GetInstance().Database[name];
                            var card = (MTGCardDefinition)Utilities.DeepCopy(srccard);
                            for (int i = 0; i < quantity; i++)
                            {
                                if (!isSideBoard) deck.MainDeck.Add(card);
                                else deck.Sideboard.Add(card);
                            }
                        }
                        else
                        {
                            var regexSB = new Regex(@"^$");
                            var m2 = regexSB.Match(line);
                            if (m2.Success) isSideBoard = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Invalid Deck File format.");
            }
            return deck;
        }
        private static TXTFormatReader instance;
    }
}
