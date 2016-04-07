using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class ChangeStateAction : MTGAction
    {
        private MTGPermanent Permanent;
        private MTGPermanentState State;
        private bool IsSet;
        private bool Changed;

        /// <summary>
        /// パーマネントの状態を変更するアクション
        /// </summary>
        /// <param name="permanent">対象パーマネント</param>
        /// <param name="state">変更対象の状態</param>
        /// <param name="isSet">True:設定, False:解除</param>
        public ChangeStateAction(MTGPermanent permanent, MTGPermanentState state, bool isSet)
        {
            Permanent = permanent;
            State = state;
            IsSet = isSet;
        }

        // アクションを行う
        public override void DoAction(GameState game)
        {
            Changed = Permanent.HasState(State) != IsSet;
            if (Changed)
            {
                if (IsSet)
                {
                    Permanent.State.Add(State);
                }
                else
                {
                    Permanent.State.Remove(State);
                }
                game.SetStateCheckRequired();
            }
        }

        // アクションを戻す
        public override void UndoAction(GameState game)
        {
            if (Changed)
            {
                Permanent.State.Remove(State);
            }
            else
            {
                Permanent.State.Add(State);
            }
        }
        
        /// <summary>
        /// パーマネントに状態をセット
        /// </summary>
        /// <param name="permanent">対象パーマネント</param>
        /// <param name="state">変更対象の状態</param>
        public static ChangeStateAction Set(MTGPermanent permanent, MTGPermanentState state)
            => new ChangeStateAction(permanent, state, true);
        
        /// <summary>
        /// パーマネントの状態をクリア
        /// </summary>
        /// <param name="permanent">対象パーマネント</param>
        /// <param name="state">変更対象の状態</param>
        public static ChangeStateAction Clear(MTGPermanent permanent, MTGPermanentState state)
            => new ChangeStateAction(permanent, state, false);
    }
}
