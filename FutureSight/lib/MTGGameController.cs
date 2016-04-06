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
            while (game.AdvanceToNextEventWithChoice())
            {
                var nextEvent = game.GetNextEvent();
                var choiceResults = new MTGChoiceResults();
                choiceResults.Results = GetAIChoiceResult(nextEvent);
                Console.WriteLine("Sleep");
                System.Threading.Thread.Sleep(10000);
                game.ExecuteNextEvent(choiceResults);
            }
        }
        
        private List<object> GetAIChoiceResult(MTGEvent nextEvent)
        {
            var player = nextEvent.Player;
            var ai = player.GetAI();
            return ai.FindNextEventChoiceResults(game, player);
        }
    }
}
