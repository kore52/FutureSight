using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FutureSight.lib
{
	public enum GamePhase
    {
        UntapStep,
        UpkeepStep,
        DrawStep,
        Main1,
        PreCombatStep,
        DeclareAttackerStep,
        DeclareBlockerStep,
        CombatDamageStep,
        EndOfCombatStep,
        Main2,
        EndStep,
        CleanupStep
    }
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
            canPutLand = true;
		}
		
        public void Calc()
        {
            // 取れうる行動を列挙

            // 手札
            foreach (var item in Players[GetActivePlayer()].Hand)
            {
                item
            }
        }

		public int GetActivePlayer() { return turnQueue.First(); }
		public List<Player> Players { get; set; }
		
		public void ForwardTurn() { turns++; }
		public int GetElapsedTurn() { return turns; }
		
		private List<int> players;
		private LinkedList<int> turnQueue;
		private int turns;
		private int step;
        private bool canPutLand;

        private List<string> nextMove;
	}
	
    //-------------------------------------------------------------
    // ディープコピー機能を持った拡張メソッド
    //-------------------------------------------------------------
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