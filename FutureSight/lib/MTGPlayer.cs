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
        static int Sequencer = 0;
        public MTGPlayer()
        {
            ID = Sequencer;
            Sequencer++;

            Hand = new List<MTGCard>();
            Graveyard = new List<MTGCard>();
            Exile = new List<MTGCard>();
            Library = new List<MTGCard>();
            Sideboard = new List<MTGCard>();
            Permanents = new List<MTGPermanent>();

            Life = 20;
            Poison = 0;
            ManaPool = new List<int>() { 0, 0, 0, 0, 0, 0 };
        }

        public string Name { get; private set; }
        public int ID { get; private set; }
        public int Life { get; private set; }
        public int Poison { get; private set; }

        public bool IsWin { get; private set; } = false;
        public bool IsLose { get; private set; } = false;
        public bool IsEmptyDraw { get; private set; } = false;

        public bool CanPlayLand { get { return (maxPlayableLand > countPlayedLand) ? true : false; } }
        private int maxPlayableLand = 1;
        private int countPlayedLand = 0;

        public bool HasAbility(MTGAbilityType type) { return ability.HasFlag(type);  }

        // ��D
        public List<MTGCard> Hand { get; private set; }
        
        // ��n
        public List<MTGCard> Graveyard; { get; private set; }
        
        // �Ǖ��̈�
        public List<MTGCard> Exile; { get; private set; }
        
        // ���C�u�����[
        public List<MTGCard> Library; { get; private set; }
        
        // �T�C�h�{�[�h
        public List<MTGCard> Sideboard; { get; private set; }
        
        // �R���g���[�����Ă���p�[�}�l���g
        public List<MTGPermanent> Permanents; { get; private set; }

        // �}�i�v�[��
        public List<int> ManaPool; { get; private set; }

        // �ΐ푊��
        public List<MTGPlayer> Opponents { get; } }

        // �v���C���[��������
        private MTGAbilityType ability;
    }
}