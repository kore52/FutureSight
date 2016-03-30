using System;
using System.Collections.Generic;
using System.Linq;

namespace FutureSight.lib
{
    interface IGameController
    {
        public void RunGame();
    }
    
    public class MTGGameController : IGameController
    {
        private GameState game;
        private int maxTime;
        
        public MTGGameController(GameState game, int maxTime)
        {
            this.game = game;
            this.maxTime = maxTime;
        }
        
        public void RunGame()
        {
        }
        
        public 
    }
}
