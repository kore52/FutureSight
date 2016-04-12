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
        public MTGCardList Hand { get; set; }

        // ��n
        public MTGCardList Graveyard { get; set; }

        // �Ǖ��̈�
        public MTGCardList Exile { get; set; }

        // ���C�u�����[
        public MTGCardList Library { get; set; }

        // �T�C�h�{�[�h
        public MTGCardList Sideboard { get; set; }

        // �R���g���[�����Ă���p�[�}�l���g
        public List<MTGPermanent> Permanents { get; set; }

        // �}�i�v�[��
        public List<int> ManaPool { get; set; }

        // ��D�̍ő�l
        public int MaximumHandSize { get; set; } = 7;

        // �������C�t
        public int Life { get; private set; } = 20;

        // �s�k����ŃJ�E���^�[�̌�
        public int LosePoisonCounter { get; private set; } = 10;

        // �v���C���[��
        public string Name { get; private set; }

        // �v���C���[�̎��J�E���^�[
        private Dictionary<MTGCounterType, int> Counters;
        public int GetCounters(MTGCounterType type) => Counters.GetValue(type, 0);

        // �ΐ푊��
        public List<MTGPlayer> Opponents { get; }

        // �v���C���[�̏��
        public List<MTGPlayerState> State { get; set; }

        public MTGGame CurrentGame { get; set; }
        public long ID
        {
            get
            {
                long id = 0;
                id += (long)Life              * 1000000000;
                id += (long)GetCounters(MTGCounterType.Poison)  * 100000000;
                id += (long)Hand.Count        * 1000000;
                id += (long)Permanents.Count  * 10000;
                id += (long)Graveyard.Count   * 1000;
                id += (long)Library.Count     * 100;
                id += (long)Exile.Count;
                return id;
            }
        }

        public bool CanPlayLand { get { return (maxPlayableLand > countPlayedLand) ? true : false; } }
        private int maxPlayableLand = 1;
        private int countPlayedLand = 0;
        public bool HasState(MTGPlayerState type) { return State.Contains(type);  }

        public bool IsAI { get; }
        public AI2 GetAI() => ai;
        private AI2 ai;

        public MTGPlayer()
        {
            Hand = new MTGCardList();
            Graveyard = new MTGCardList();
            Exile = new MTGCardList();
            Library = new MTGCardList();
            Sideboard = new MTGCardList();
            Permanents = new List<MTGPermanent>();
            State = new List<MTGPlayerState>();
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
        
        /// <summary>
        /// ��D�A��n�A�p�[�}�l���g����v���C��N�����\�Ȕ\�͈ꗗ���擾����
        /// </summary>
        public List<MTGSource> GetPlayableActivations()
        {
            var acts = new List<MTGSource>();
//            var acts = Hand.Select(card => card.CanPlay()).
            foreach (var card in Hand) acts.AddRange(card.GetActivations());
            foreach (var card in Graveyard) acts.AddRange(card.GetActivations());
            foreach (var card in Permanents) acts.AddRange(card.GetActivations());
            return acts;
        }
        
        // �v���C���[�̏󋵋N�������i�s�k�`�F�b�N�j
        public void GenerateStateBasedActions()
        {
            if (Life <= 0)
            {
                // TODO
            }

            if (GetCounters(MTGCounterType.Poison) >= LosePoisonCounter)
            {
                // TODO
            }
        }

        // �v���C���[�̎�D�ƃ��C�u�����[������
        public static void PrepareHandAndLibrary(MTGPlayer player, MTGDeck deck)
        {
            // ���C�u�����[�̏���
            player.CreateLibrary(deck);

            // �V���b�t��
            Utilities.Shuffle(player.Library);

            // MaximumHandSize�����ŏ��Ƀh���[
            for (int i = 0; i < player.MaximumHandSize; i++)
            {
                if (player.Library.Count != 0)
                {
                    var card = player.Library[0];
                    player.Library.RemoveAt(0);
                    player.Hand.Add(card);
                }
            }
        }

        /// <summary>
        /// �f�b�L�I�u�W�F�N�g���烉�C�u�����[�ƃT�C�h�{�[�h���쐬
        /// </summary>
        /// <param name="deck">�v���C���[���g�p����f�b�L</param>
        private void CreateLibrary(MTGDeck deck)
        {
            foreach (var c in deck.MainDeck)
            {
                Library.Add(new MTGCard(c, this));
            }

            foreach (var c in deck.Sideboard)
            {
                Sideboard.Add(new MTGCard(c, this));
            }
        }
        
        public bool IsLoseGame()
            => State.Contains(MTGPlayerState.LoseGame);
    }
}