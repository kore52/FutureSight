using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    [Serializable()]
    public class MTGPermanent
    {
        public MTGPermanent()
        {
            card = new MTGCard();
            ID = generateId(card);
        }

        public MTGPermanent(MTGCard fromCard)
        {
            card = fromCard;
            ID = generateId(card);
        }

        public PermanentType PermanentType { get; set; }
        public int ID { get; }

        public string           Name { get { return card.Name; } }
        public PermanentType    Type { get { return card.CardType.GetPermanentType(); } }
        public List<MTGSubType> SubType { get { return card.SubType; } }
        public List<MTGSpecialType> SpecialType { get { return card.SpecialType; } }

        /// パーマネントID生成
        private int generateId(MTGCard card)
        {
            var str = card.Name + (new System.Random()).Next();
            int id = MurMurHash3.Hash(new MemoryStream(Encoding.Unicode.GetBytes(str)));
            return id;
        }

        private MTGCard card;
    }
}
