using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FutureSight.lib
{
    public class MTGDeck : List<MTGCardDefinition>
    {
    }
    
    public class DeckLoader
    {
        public static MTGDeck LoadFromFile(string filename)
        {
            if (Regex.Match(filename.ToLower(), "\\.dek$").Success) return DEKFormatReader.GetInstance().Load(filename);
            else if (Regex.Match(filename.ToLower(), "\\.txt$").Success) return TXTFormatReader.GetInstance().Load(filename);
            throw Exception("Invalid Deck File extension.");
        }
    }
    
    interface IDeckFileFormatReader
    {
        MTGDeck Load(string filename);
    }
    
    public class DEKFormatReader : IDeckFileFormatReader
    {
        public MTGDeck Load(string filename)
        {
            try {
                MTGDeck deck = new MTGDeck();
                var doc = XDocument.Load(filename);
                foreach (var elem in doc.Descendants("Deck"))
                {
                    for ( int i = 0; i < int.Parse(elem.Attribute("Quantity")); i++ )
                    {
                        bool isSideBoard = (Regex.Match(elem.Attribute("Sideboard").ToLower(), "true")) ? true : false;
                        var name = elem.Attribute("Name");
                        
                        var card = (MTGCardDefinition)Utilities.DeepCopy(MTGCardInfoLoader.Database[name]);
                        deck.Add(card);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Invalid Deck File format.");
            }
            
            return deck;
        }
    }
    
    public class TXTFormatReader : IDeckFileFormatReader
    {
        public MTGDeck Load(string filename)
        {
            MTGDeck deck;
            return deck;
        }
    }
}
