using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FutureSight.lib
{
    [Serializable()]
    public class MTGPlayer : MTGTarget
    {
        // ��D
        public List<MTGCard> Hand { get; set; }

        // ��n
        public List<MTGCard> Graveyard { get; set; }

        // �Ǖ��̈�
        public List<MTGCard> Exile { get; set; }

        // ���C�u�����[
        public List<MTGCard> Library { get; set; }

        // �T�C�h�{�[�h
        public List<MTGCard> Sideboard { get; set; }

        // �R���g���[�����Ă���p�[�}�l���g
        public List<MTGPermanent> Permanents { get; set; }

        // �}�i�v�[��
        public List<int> ManaPool { get; set; }

        // �ΐ푊��
        public List<MTGPlayer> Opponents { get; }

        // �v���C���[��������
        private MTGAbilityType ability;
        public GameState CurrentGame { get; set; }
        public string Name { get; private set; }
        public int Life { get; private set; } = 20;
        public Dictionary<MTGCounterType, int> Counters { get; private set; }
        public int LosePoisonCounter { get; private set; } = 10;
        public long ID
        {
            get
            {
                long id = 0;
                id += (long)Life              * 1000000000;
                id += (long)Counters[MTGCounterType.Poison]  * 100000000;
                id += (long)Hand.Count        * 1000000;
                id += (long)Permanents.Count  * 10000;
                id += (long)Graveyard.Count   * 1000;
                id += (long)Library.Count     * 100;
                id += (long)Exile.Count;
                return id;
            }
        }
        public bool IsWin { get; private set; } = false;
        public bool IsLose { get; private set; } = false;
        public bool IsEmptyDraw { get; private set; } = false;
        public bool CanPlayLand { get { return (maxPlayableLand > countPlayedLand) ? true : false; } }
        private int maxPlayableLand = 1;
        private int countPlayedLand = 0;
        public bool HasAbility(MTGAbilityType type) { return ability.HasFlag(type);  }

        public bool IsAI { get; }
        private AI2 ai;

        public MTGPlayer()
        {
            Hand = new List<MTGCard>();
            Graveyard = new List<MTGCard>();
            Exile = new List<MTGCard>();
            Library = new List<MTGCard>();
            Sideboard = new List<MTGCard>();
            Permanents = new List<MTGPermanent>();
            ManaPool = new List<int>() { 0, 0, 0, 0, 0, 0 };
            Counters = new Dictionary<MTGCounterType, int>();
        }
        
        public MTGPlayer(int initialLife, int losePoisonCounter, bool isAI = true) : this()
        {
            Life = initialLife;
            LosePoisonCounter = losePoisonCounter;
            IsAI = isAI;
            if (isAI) { ai = new AI2(); }
        }

        // �v���C���[�̏󋵋N�������i�s�k�`�F�b�N�j
        public void GenerateStateBasedActions()
        {
            if (Life <= 0)
            {
                // TODO
            }

            if (Counters[MTGCounterType.Poison] >= LosePoisonCounter)
            {
                // TODO
            }
        }

        // �v���C���[�̎�D�ƃ��C�u�����[������
        public static void PrepareHandAndLibrary(MTGPlayer player, MTGDeck deckList)
        {
            // �f�b�L���X�g�̓ǂݍ���
            // �V���b�t��

            // InitialHandSize�����h���[

        }
    }
}