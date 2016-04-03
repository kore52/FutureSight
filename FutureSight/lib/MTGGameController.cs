using System;
using System.Collections.Generic;
using System.Linq;

namespace FutureSight.lib
{
    interface IGameController
    {
        void RunGame();
    }
    
    public class SinglePlayGameController : IGameController
    {
        private GameState game;
        private int maxTime;
        
        public SinglePlayGameController(GameState game, int maxTime)
        {
            this.game = game;
            this.maxTime = maxTime;
        }
        
        public void RunGame()
        {
        }
    }

    public class AutoPlayGameController : IGameController
    {
        private GameState game;
        private int maxTime;

        public AutoPlayGameController(GameState game, int maxTime)
        {
            this.game = game;
            this.maxTime = maxTime;
        }

        public void RunGame()
        {
            while (true)
            {
                var mtgEvent = game.GetNextEvent();
            }
        }
    }
}
