using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    /// <summary>
    /// プレイできる呪文や効果一覧を探索するクラス
    /// </summary>
    public class MTGPlayChoice : MTGChoice
    {
        /// <summary>
        /// 「優先権のパス」行動を定義するオブジェクト
        /// </summary>
        static public MTGChoiceResults PassOptions;

        static private MTGChoice instance;

        /// <summary>
        /// AI向け選択肢一覧を探索
        /// </summary>
        /// <returns>行動候補一覧</returns>
        public override MTGChoiceResults GetAIOptions(MTGGame game, MTGEvent aEvent)
        {
            var options = new MTGChoiceResults();
            options.Add(MTGPlayChoiceResult.Pass);
            foreach (var act in aEvent.Player.GetPlayableActivations())
                options.Add(act);

            return PassOptions;
        }

        public override bool IsValid()
            => false;
            
        static public MTGChoice GetInstance()
        {
            if (instance == null) instance = new MTGPlayChoice();
            return instance;
        }

        static MTGPlayChoice()
        {
            PassOptions = new MTGChoiceResults(){ MTGPlayChoiceResult.Pass };
        }
    }
}
