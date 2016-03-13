using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FutureSight.lib
{
    [Serializable()]
    public class Player
	{
        static int Sequencer = 0;
        public Player()
		{
            ID = Sequencer;
            Sequencer++;

			Hand = new List<int>();
			Graveyard = new List<int>();
			Exile = new List<int>();
			Library = new List<int>() { 1,1,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
			Sideboard = new List<int>() { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 };
            Permanents = new List<Permanent>();

            Life = 20;
            Poison = 0;
            ManaPool = new List<int>() { 0, 0, 0, 0, 0, 0 };
		}
		
		public int ID { get; set; }
		public List<int> Hand { get; set; }
		public List<int> Graveyard { get; set; }
		public List<int> Exile { get; set; }
		public List<int> Library { get; set; }
		public List<int> Sideboard { get; set; }
        public List<Permanent> Permanents { get; set; }

        public int Life { get; set; }
        public int Poison { get; set; }
		public List<int> ManaPool { get; set; }

        public bool IsWin { get; set; } = false;
        public bool IsLose { get; set; } = false;
        public bool IsEmptyDraw { get; set; } = false;

		public void ShuffleLibrary() {}
		public void DrawCard()
		{
			Hand.Add(Library[0]);
			Library.Remove(Library[0]);
		}
		public void PutLand(int no) {}
		public void CastSpell(int no) {}
	}
}