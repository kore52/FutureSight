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
		}
	}
}