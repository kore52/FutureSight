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
			turns = 0;
			turnQueue = new LinkedList<int>();
			turnQueue.AddLast(0);
			turnQueue.AddLast(1);
			canPlayLand = true;

            Players = new List<Player>();

            step = GamePhase.Main1;
		}
		
		public void Calc()
		{
            nextMove = new List<string>();
			Player player = Players[GetActivePlayer()];
			for (int i = 0; i < player.Hand.Count; i++)
			{
				Card item = CardDB.GetInstance().get(player.Hand[i]);
				switch(step) {
				case GamePhase.UntapStep: break;
				case GamePhase.UpkeepStep:
					if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayer() + ":cast:" + i); }
					break;
				case GamePhase.DrawStep:
				case GamePhase.Main1:
					if (item.CardType.HasFlag(CardType.Land) && canPlayLand) { nextMove.Add(GetActivePlayer() + ":play:" + i); }
					break;
				case GamePhase.PreCombatStep:
				case GamePhase.DeclareAttackerStep:
				case GamePhase.DeclareBlockerStep:
				case GamePhase.CombatDamageStep:
				case GamePhase.EndOfCombatStep:
				case GamePhase.Main2:
					if (item.CardType.HasFlag(CardType.Land) && canPlayLand) { nextMove.Add(GetActivePlayer() + ":play:" + i); }
					break;
				case GamePhase.EndStep:
				case GamePhase.CleanupStep: break;

				}
				
			}
			nextMove.Add(GetActivePlayer() + ":pass");

#if DEBUG
            System.Console.WriteLine("move candidates:");
            foreach (var nm in nextMove) { System.Console.WriteLine(nm); }
#endif
        }

		
		public int GetActivePlayer() { return turnQueue.First(); }
		public List<Player> Players { get; set; }
		
		public void ForwardTurn() { turns++; }
		public int GetElapsedTurn() { return turns; }
		
		private LinkedList<int> turnQueue;
		private int turns;
		private GamePhase step;
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
            int max = player.ManaPool.Sum();
            List<int> costList = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
            List<int> tmp = new List<int>(player.ManaPool);

			MatchCollection mc = Regex.Matches(cost, @"\{(.?)\}");
			foreach (System.Text.RegularExpressions.Match m in mc)
			{
				switch (m.Value)
				{
				    case "W": costList[(int)Color.White]++; break;
				    case "U": costList[(int)Color.Blue]++; break;
                    case "B": costList[(int)Color.Black]++; break;
                    case "R": costList[(int)Color.Red]++; break;
                    case "G": costList[(int)Color.Green]++; break;
                    case "C": costList[(int)Color.Colorless]++; break;
                    case "X":
                    case "Y":
                    case "Z":
                        break;
				    default:
                        // 不特定マナ
                        costList[(int)Color.Generic] += int.Parse(m.Value);
                        break;
				}
			}

            tmp[0] = (tmp[0] - costList[0] >= 0) ? tmp[0] - costList[0] : 0;
            tmp[1] = (tmp[1] - costList[1] >= 0) ? tmp[1] - costList[1] : 0;
            tmp[2] = (tmp[2] - costList[2] >= 0) ? tmp[2] - costList[2] : 0;
            tmp[3] = (tmp[3] - costList[3] >= 0) ? tmp[3] - costList[3] : 0;
            tmp[4] = (tmp[4] - costList[4] >= 0) ? tmp[4] - costList[4] : 0;
            tmp[5] = (tmp[5] - costList[5] >= 0) ? tmp[5] - costList[5] : 0;

            if (costList[(int)Color.White] <= player.ManaPool[(int)Color.White] &&
                costList[(int)Color.Blue] <= player.ManaPool[(int)Color.Blue] &&
                costList[(int)Color.Black] <= player.ManaPool[(int)Color.Black] &&
                costList[(int)Color.Red] <= player.ManaPool[(int)Color.Red] &&
                costList[(int)Color.Green] <= player.ManaPool[(int)Color.Green] &&
                costList[(int)Color.Colorless] <= player.ManaPool[(int)Color.Colorless] &&
                costList[(int)Color.Generic] <= tmp.Sum())
            {
                result = true;
            }
			return result;
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