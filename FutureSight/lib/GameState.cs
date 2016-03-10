using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FutureSight.lib
{
	enum BehaviorCode
	{
		PutLand,
		CastSpell
	}
	
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
			canPlayLand = true;
		}
		
		public void Calc()
		{
			Player player = Players[GetActivePlayer()];
			for (int i = 0; i < player.Hand.Length(); i++)
			{
				Card item = CardDB.GetInstance().get(player.Hand[i]);
				switch(step) {
				case UntapStep: break;
				case UpkeepStep:
					if ((item.CardType & Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayer() + ":cast:" + i); }
					break;
				case DrawStep:
				case Main1:
					if ((item.CardType & Land) && canPlayLand) { nextMove.Add(GetActivePlayer() + ":play:" + i); }
					break;
				case PreCombatStep:
				case DeclareAttackerStep:
				case DeclareBlockerStep:
				case CombatDamageStep:
				case EndOfCombatStep:
				case Main2:
					if ((item.CardType & Land) && canPlayLand) { nextMove.Add(GetActivePlayer() + ":play:" + i); }
					break;
				case EndStep:
				case CleanupStep: break;
				default:
				}
				
			}
			nextMove.Add(GetActivePlayer() + ":pass");
		}

		
		public int GetActivePlayer() { return turnQueue.First(); }
		public List<Player> Players { get; set; }
		
		public void ForwardTurn() { turns++; }
		public int GetElapsedTurn() { return turns; }
		
		private List<int> players;
		private LinkedList<int> turnQueue;
		private int turns;
		private int step;
		private bool canPlayLand;

		private List<string> nextMove;
		
		private bool IsTargetExists()
		{
			return false;
		}
		
		private bool IsManaCostSatisfied(string cost, Player player)
		{
			bool result = false;
			
			// calc max mana in manapool
			int max = player.ManaPool(White) + player.ManaPool(Blue) + player.ManaPool(Black) + player.ManaPool(Red) + player.ManaPool(Green) + player.ManaPool(Colorless);
			int cmc = 0;
			MatchCollection mc = Regex.Matches(ManaCost, @"\{(.+)\}");
			foreach (RegularExpressions.Match m in mc)
			{
				switch (m.Value)
				{
				case "W":
					if (player.ManaPool(White)
				case "U":
				case "B":
				case "R":
				case "G":
				case "C":
					cmc++;
					break;
				case "X":
				case "Y":
				case "Z":
					break;
				default:
				}
			}
			return _result;
		}
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