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
        public List<MTGCard> Hand { get; private set; }

        // 墓地
        public List<MTGCard> Graveyard { get; private set; }

        // 追放領域
        public List<MTGCard> Exile { get; private set; }

        // ライブラリー
        public List<MTGCard> Library { get; private set; }

        // サイドボード
        public List<MTGCard> Sideboard { get; private set; }

        // コントロールしているパーマネント
        public List<MTGPermanent> Permanents { get; private set; }

        // マナプール
        public List<int> ManaPool { get; private set; }

        // 対戦相手
        public List<MTGPlayer> Opponents { get; }

        // プレイヤーが持つ効果
        private MTGAbilityType ability;

        public GameState CurrentGame { get; set; }

        public MTGPlayer()
        {
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
        public int Life { get; private set; }
        public int Poison { get; private set; }

        public long ID
        {
            get
            {
                long id = 0;
                id += Life              * 1000000000;
                id += Poison            * 100000000;
                id += Hand.Count        * 1000000;
                id += Permanents.Count  * 10000;
                id += Graveyard.Count   * 1000;
                id += Library.Count     * 100;
                id += Exile.Count;
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

        // プレイヤーの状況起因処理（敗北チェック）
        public void GenerateStateBasedActions()
        {
            if (Life <= 0)
            {
                // TODO
            }

            if (Poison >= 10)
            {
                // TODO
            }
        }

    }
}