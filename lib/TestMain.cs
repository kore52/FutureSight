using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FutureSight.lib
{
	
	public static class Hello
	{
		public static void Main()
		{
			GameState aa = new GameState();
			System.Console.WriteLine(aa.GetElapsedTurn());
			
			aa.ForwardTurn();
			GameState bb = (GameState)aa.DeepCopy();
			System.Console.WriteLine(bb.GetElapsedTurn());
			
			Player p1 = new Player();
			p1.DrawCard();
			p1.DrawCard();
			p1.DrawCard();
			string s = p1.Hand.Join();
			System.Console.WriteLine(s);
		}
	}
	public static class Extensions {
	    public static string Join(this List<int> list) {
	        return string.Join(",", list.Select(item => item.ToString()).ToArray());
	    }
	}
}