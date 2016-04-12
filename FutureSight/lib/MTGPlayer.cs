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
        // 手札
        public MTGCardList Hand { get; set; }

        // 墓地
        public MTGCardList Graveyard { get; set; }

        // 追放領域
        public MTGCardList Exile { get; set; }

        // ライブラリー
        public MTGCardList Library { get; set; }

        // サイドボード
        public MTGCardList Sideboard { get; set; }

        // コントロールしているパーマネント
        public List<MTGPermanent> Permanents { get; set; }

        // マナプール
        public List<int> ManaPool { get; set; }

        // 手札の最大値
        public int MaximumHandSize { get; set; } = 7;

        // 初期ライフ
        public int Life { get; private set; } = 20;

        // 敗北する毒カウンターの個数
        public int LosePoisonCounter { get; private set; } = 10;

        // プレイヤー名
        public string Name { get; private set; }

        // プレイヤーの持つカウンター
        private Dictionary<MTGCounterType, int> Counters;
        public int GetCounters(MTGCounterType type) => Counters.GetValue(type, 0);

        // 対戦相手
        public List<MTGPlayer> Opponents { get; }

        // プレイヤーの状態
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
        /// 手札、墓地、パーマネントからプレイや起動が可能な能力一覧を取得する
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
        
        // プレイヤーの状況起因処理（敗北チェック）
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

        // プレイヤーの手札とライブラリーを準備
        public static void PrepareHandAndLibrary(MTGPlayer player, MTGDeck deck)
        {
            // ライブラリーの準備
            player.CreateLibrary(deck);

            // シャッフル
            Utilities.Shuffle(player.Library);

            // MaximumHandSizeだけ最初にドロー
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
        /// デッキオブジェクトからライブラリーとサイドボードを作成
        /// </summary>
        /// <param name="deck">プレイヤーが使用するデッキ</param>
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