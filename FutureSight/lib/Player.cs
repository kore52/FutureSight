using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FutureSight.lib
{
	public class Player
	{
		public Player()
		{
			Random seed = new Random();
			ID = seed.Next();
			Hand = new List<int>();
			Graveyard = new List<int>();
			Exile = new List<int>();
			Library = new List<int>() { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
			Sideboard = new List<int>() { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 };
		}
		
		public int ID { get; set; }
		public List<int> Hand { get; set; }
		public List<int> Graveyard { get; set; }
		public List<int> Exile { get; set; }
		public List<int> Library { get; set; }
		public List<int> Sideboard { get; set; }
		
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