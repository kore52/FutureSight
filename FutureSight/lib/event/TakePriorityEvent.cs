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
    public class TakePriorityEvent : MTGEvent
    {
        public TakePriorityEvent(MTGPlayer player)
            : base(null, player, null, null, EventAction, "Take priority") { }
        
        public static MTGEventAction EventAction;
        
        static TakePriorityEvent()
        {
            EventAction = new MTGEventAction((MTGGame game, MTGEvent aEvent) =>
            {
                var playChoiceResult = aEvent.Chosen;
                
                // 行動の選択肢が「優先権のパス」であれば、優先権を持つプレイヤーを変更
                if (playChoiceResult.Equals(MTGPlayChoiceResult.Pass))
                {
                    // どのプレイヤーも優先権をパスしたなら効果の解決
                    if (game.PriorityPassed)
                    {
                        game.ClearPriorityPassed();
                        game.Resolve();
                    }
                    else
                    {
                        // 優先権の変更
                        game.SetPriorityPassed();
                        switch (game.Step)
                        {
                            case MTGStep.ActivePlayer:
                                game.Step = MTGStep.OtherPlayer;
                                break;
                            case MTGStep.OtherPlayer:
                                game.Step = MTGStep.ActivePlayer;
                                break;
                        }
                    }
                }
                else
                {
                    // 選択を用いないコストの支払い: {T}:等
                    foreach (var costEvent in playChoiceResult.SourceActivation.CostEvent)
                    {
                        if (!costEvent.HasChoice())
                            game.ExecuteEvent(costEvent);
                    }
                    
                    // 選択を用いるコストの支払い: マナコスト等
                    foreach (var costEvent in playChoiceResult.SourceActivation.CostEvent)
                    {
                        if (costEvent.HasChoice())
                            game.ExecuteEvent(costEvent);
                    }
                    
                    // 選択した行動の実行
                    game.AddEvent(playChoiceResult.Event);
                }
            });
        }
    }
}