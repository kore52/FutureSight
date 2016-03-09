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
			activePlayer = 0;
			turns = 0;
		}
		
		//public Player GetActivePlayer() {}
		//public List<Player> Player { get; set; }
		
		public void ForwardTurn() { turns++; }
		public int GetElapsedTurn() { return turns; }
		
		private int activePlayer;
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