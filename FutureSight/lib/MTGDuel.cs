using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class MTGDuel
    {
        private int numDuels;
        private GameState game;
        
        public GameState PrepareNextGame()
        {
            // �v���C���[�̍쐬
            MTGPlayer me = new MTGPlayer(20, 10, true);
            MTGPlayer opponent = new MTGPlayer(20, 10, true);
            List<MTGPlayer> players = new List<MTGPlayer>() { me, opponent };

            // �Q�[���̍쐬
            game = GameState.CreateGame(players, me);
            
            // �v���C���[�̃��C�u�����[����ю�D�̏���
            MTGPlayer.PrepareHandAndLibrary(me, DeckLoader.GetInstance().LoadFromFile("resources\\deck1.txt"));
            MTGPlayer.PrepareHandAndLibrary(opponent, DeckLoader.GetInstance().LoadFromFile("resources\\deck1.txt"));
            
            return game;
        }
    }
}
