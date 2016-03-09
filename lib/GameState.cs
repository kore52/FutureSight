using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FutureSight.lib
{
	[Serializable()]
	public class GameState
	{
		public GameState()
		{
			players = new List<int>() { 0, 1 };
			
			turns = 0;
			turnQueue = new LinkedList<int>();
			turnQueue.AddLast(0);
			turnQueue.AddLast(1);
		}
		
		public int GetActivePlayer() { return turnQueue.First(); }
		public List<Player> Player { get; set; }
		
		public void ForwardTurn() { turns++; }
		public int GetElapsedTurn() { return turns; }
		
		private List<int> players;
		private LinkedList<int> turnQueue;
		private int turns;
		private int step;
	}
	
	static class DeepCopyUtils
	{
		public static object DeepCopy(this object target)
		{
			object result;
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream mem = new MemoryStream();

			try
			{
				b.Serialize(mem, target);
				mem.Position = 0;
				result = b.Deserialize(mem);
			}
			finally
			{
				mem.Close();
			}
			return result;
		}
	}
}