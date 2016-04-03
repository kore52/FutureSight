using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

namespace FutureSight.lib
{
    public class MTGEvent
    {
        public static MTGEvent NoChoiceResults = new MTGEvent();

        /// <summary>
        /// イベントのコントロールプレイヤー
        /// </summary>
        public MTGPlayer Player { get; private set; }

        /// <summary>
        /// イベントの選択肢
        /// </summary>
        public MTGChoice Choice { get; private set; }

        /// <summary>
        /// イベントの対象プレイヤー・パーマネント
        /// </summary>
        public MTGTarget Target { get; private set; }

        /// <summary>
        /// イベントのアクション
        /// </summary>
        public MTGEventAction Action { get; private set; }

        public MTGEvent()
        {
            
        }

        public MTGEvent(MTGPlayer player, MTGTarget target, MTGEventAction action)
        {
            Player = player;
            Target = target;
            Action = action;
        }

        public void Execute(GameState game, MTGChoiceResults choiceResults)
        {
            Action.ExecuteEvent(game, this);
        }

        public bool HasChoice()
        {
            return Choice != null;
        }

        /// <summary>
        /// イベント選択肢の結果を配列で返す。
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public MTGChoiceResults GetChoiceResults(GameState game)
        {
            return Choice.GetChoiceResults(game, this);
        }
    }
    
    interface MTGCost
    {
    }
    
    public class MTGDrawEvent : MTGEvent
    {
    }
}