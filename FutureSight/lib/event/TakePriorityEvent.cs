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
                
                // �s���̑I�������u�D�挠�̃p�X�v�ł���΁A�D�挠�����v���C���[��ύX
                if (playChoiceResult.Equals(MTGPlayChoiceResult.Pass))
                {
                    // �ǂ̃v���C���[���D�挠���p�X�����Ȃ���ʂ̉���
                    if (game.PriorityPassed)
                    {
                        game.ClearPriorityPassed();
                        game.Resolve();
                    }
                    else
                    {
                        // �D�挠�̕ύX
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
                    // �I����p���Ȃ��R�X�g�̎x����: {T}:��
                    foreach (var costEvent in playChoiceResult.SourceActivation.CostEvent)
                    {
                        if (!costEvent.HasChoice())
                            game.ExecuteEvent(costEvent);
                    }
                    
                    // �I����p����R�X�g�̎x����: �}�i�R�X�g��
                    foreach (var costEvent in playChoiceResult.SourceActivation.CostEvent)
                    {
                        if (costEvent.HasChoice())
                            game.ExecuteEvent(costEvent);
                    }
                    
                    // �I�������s���̎��s
                    game.AddEvent(playChoiceResult.Event);
                }
            });
        }
    }
}