using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    [Serializable()]
    public class MTGPermanent : MTGSource
    {
        public PermanentType PermanentType { get; set; }
        public int ID { get; }
        private MTGCard card;

        public string                           Name { get { return card.Name; } }
        public PermanentType                    Type { get { return card.CardType.GetPermanentType(); } }
        public List<MTGSubType>                 SubType { get; set; }
        public List<MTGSpecialType>             SpecialType { get; set; }
        public MTGPlayer                        Owner { get { return card.Owner; } set { card.Owner = Owner; } }
        public MTGPlayer                        Controller { get; set; }
        public int                              Power { get; set; }
        public int                              Toughness { get; set; }
        public Dictionary<MTGCounterType, int>  Counters;
        public int                              Score { get; set; }
        public List<MTGPermanentState>          State { get; set; }

        public MTGPermanent()
        {
            card = new MTGCard();
            ID = generateId(card);
            Power = 0;
            Toughness = 0;
            Counters = new Dictionary<MTGCounterType, int>();
            Score = 0;
        }

        public MTGPermanent(MTGCard fromCard) : this()
        {
            card = fromCard;
            ID = generateId(card);
        }

        /// パーマネントID生成
        private int generateId(MTGCard card)
        {
            var str = card.Name + (new System.Random()).Next();
            int id = MurMurHash3.Hash(new MemoryStream(Encoding.Unicode.GetBytes(str)));
            return id;
        }

        /// <summary>
        /// パーマネントの状態起因チェック
        /// </summary>
        public void GenerateStateBasedActions()
        {
            var game = Owner.CurrentGame;
            if (IsCreature())
            {
                if (Toughness <= 0)
                {
                    game.AddDelayedAction(new RemoveFromBattleField(this, LocationType.Graveyard));
                }
            }
        }

        public void AddState(MTGPermanentState state)
            => State.Add(state);

        public void RemoveState(MTGPermanentState state)
            => State.Remove(state);

        public bool IsArtifact()
            => PermanentType.HasFlag(PermanentType.Artifact);

        public bool IsCreature()
            => PermanentType.HasFlag(PermanentType.Creature);

        public bool IsEnchantment()
            => PermanentType.HasFlag(PermanentType.Enchantment);

        public bool IsLand()
            => PermanentType.HasFlag(PermanentType.Land);

        public bool IsPlaneswalker()
            => PermanentType.HasFlag(PermanentType.Planeswalker);

        public bool HasSubType(MTGSubType subType)
            => SubType.Contains(subType);

        public bool IsEquipment()
            => IsArtifact() && HasSubType(MTGSubType.Equipment);

        public bool IsAura()
            => IsEnchantment() && HasSubType(MTGSubType.Aura);

        public bool HasState(MTGPermanentState state)
            => State.Contains(state);

        public bool IsTapped()
            => HasState(MTGPermanentState.Tapped);

        public bool IsUntapped()
            => !IsTapped();
    }
}
