using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class MTGPlayerConfig
    {
        private AI2 ai;
        private MTGPlayer player;
        private bool isAI;
        
        public MTGPlayerConfig(bool isAI = true)
        {
            this.isAI = isAI;
            Setup();
        }
        
        private void Setup()
        {
            if (isAI)
            {
                ai = new AI2();
            }
        }
    }
    
    public class MTGDuel
    {
        private int numDuels;
        private GameState game;
        
        public MTGDuel(MTGDuelConfig duelConfig)
        {
            game = new GameState();
        }
        
        public void SetPlayers(MTGPlayerConfig config)
        {
            game.Player
        }
        
        public GameState PrepareNextGame()
        {
            // プレイヤーの作成
            MTGPlayer me = new MTGPlayer(20, 10);
            MTGPlayer opponent = new MTGPlayer(20, 10);
            
            
            
            // ゲームの作成
            
            // プレイヤーのライブラリーおよび手札の準備
            return game;
        }
    }
}
