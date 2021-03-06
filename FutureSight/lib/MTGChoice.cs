﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public abstract class MTGChoice
    {
        /// <summary>
        /// AI向けの選択肢一覧を返す仮想メソッド
        /// </summary>
        public abstract MTGChoiceResults GetAIOptions(MTGGame game, MTGEvent mtgevent);

        public MTGChoiceResults GetChoiceResults(MTGGame game, MTGEvent mtgevent)
        {
            return GetAIOptions(game, mtgevent);
        }
        
        public virtual bool IsValid() => true;
    }
    
    public class NoneChoice : MTGChoice
    {
        public override MTGChoiceResults GetAIOptions(MTGGame game, MTGEvent mtgevent)
            => new MTGChoiceResults();
        
        public override bool IsValid() => false;
    }

    public class MTGChoiceResults : IEnumerable<object>
    {
        public AIScore Score { get; set; }

        /// <summary>
        ///  選択肢一覧の格納場所
        /// </summary>
        public List<object> Results { get; set; }

        public MTGChoiceResults()
        {
            Results = new List<object>();
        }

        public MTGChoiceResults(List<object> choices)
        {
            Results = choices;
        }

        public MTGChoiceResults(object choice) : this()
        {
            Results.Add(choice);
        }

        public void Add(object item)
        {
            Results.Add(item);
        }

        public void Remove(object item)
        {
            Results.Remove(item);
        }

        public int Count { get { return Results.Count; } }

        public IEnumerator<object> GetEnumerator()
        {
            yield return Results;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class MTGCardChoice : MTGChoice
    {
        public override MTGChoiceResults GetAIOptions(MTGGame state, MTGEvent aEvent)
        {
            var choiceResultList = new MTGChoiceResults();
            var player = state.Players[0];
            foreach (var hand in player.Hand)
            {
                choiceResultList.Add(hand);
            }
            return choiceResultList;
        }
    }
}
