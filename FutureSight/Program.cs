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

            GameTree root = new GameTree();
            root.Data = new GameState();
            root.Data.Initialize();
            while(!root.Data.IsGameFinished)
            {
                // 手番探索
                GameState.Calc(root, (int)Depth.Zero);

                // 評価値の一番高い手を進める
                foreach (var move in root.Node)
                {
                    if (move.Data.EvalScore == root.Data.EvalScore)
                    {
                        root.Data = GameState.DoMove(move.Data.CurrentMove, root.Data);

                        System.Diagnostics.Debug.Print("score:"+root.Data.EvalScore.ToString() + ", move:" + root.Data.CurrentMove);
                        System.Diagnostics.Debug.Print("move done.");
                        Console.ReadKey();

                        break;
                    }
                }

            }
        }
    }
    public static class Extensions
    {
        
    }
}
