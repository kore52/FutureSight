using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight
{
    using FutureSight.lib;

    class Program
    {
        static void Main(string[] args)
        {
            CardDB cardDB = new CardDB();
            cardDB.LoadCardDB();
            Console.WriteLine(cardDB.get(1).Name);

            GameState root = new GameState();
            System.Console.WriteLine(root.GetElapsedTurn());

            Player p1 = new Player();
            p1.DrawCard();
            p1.DrawCard();
            p1.DrawCard();
            string s = p1.Hand.Join();
            System.Console.WriteLine(s);
            root.Players.Add(p1);

            root.Calc();

            Console.ReadKey();
        }
    }
    public static class Extensions
    {
        public static string Join(this List<int> list)
        {
            return string.Join(",", list.Select(item => item.ToString()).ToArray());
        }
    }
}
