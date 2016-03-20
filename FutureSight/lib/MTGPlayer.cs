using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FutureSight.lib
{
    [Serializable()]
    public class MTGPlayer : MTGTarget
	{
        static int Sequencer = 0;
        public MTGPlayer()
		{
            ID = Sequencer;
            Sequencer++;

			Hand = new List<MTGCard>();
			Graveyard = new List<MTGCard>();
			Exile = new List<MTGCard>();
            Library = new List<MTGCard>();
            Sideboard = new List<MTGCard>();
            Permanents = new List<MTGPermanent>();

            Life = 20;
            Poison = 0;
            ManaPool = new List<int>() { 0, 0, 0, 0, 0, 0 };
		}
		

        public int ID { get; set; }
        public int Life { get; set; }
        public int Poison { get; set; }

        public bool IsWin { get; set; } = false;
        public bool IsLose { get; set; } = false;
        public bool IsEmptyDraw { get; set; } = false;
        public bool CanPlayLand { get; set; } = false;

        public bool HasAbility(MTGAbilityType type) { return ability.HasFlag(type);  }

        // 領域
        public List<MTGCard> Hand { get; set; }
        public List<MTGCard> Graveyard { get; set; }
        public List<MTGCard> Exile { get; set; }
        public List<MTGCard> Library { get; set; }
        public List<MTGCard> Sideboard { get; set; }
        public List<MTGPermanent> Permanents { get; set; }

        public List<int> ManaPool { get; set; }

        public void ShuffleLibrary() {}
		public void DrawCard()
		{
			Hand.Add(Library[0]);
			Library.Remove(Library[0]);
		}
		public void PutLand(int no) {}
		public void CastSpell(int no) {}

        // 内部

        // プレイヤーが持つ能力
        private MTGAbilityType ability;
	}
}