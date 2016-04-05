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
        static void Initialize()
        {
        }
        
        static void RunDuel()
        {
            var newDuel = new MTGDuel();
            
            var game = newDuel.PrepareNextGame();
            var controller = new AutoPlayGameController(game, 3600 * 60);
            controller.RunGame();
        }

        static void Main(string[] args)
        {
            Initialize();
            RunDuel();
        }
    }
}

