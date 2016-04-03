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
        /// �C�x���g�̃R���g���[���v���C���[
        /// </summary>
        public MTGPlayer Player { get; private set; }

        /// <summary>
        /// �C�x���g�̑I����
        /// </summary>
        public MTGChoice Choice { get; private set; }

        /// <summary>
        /// �C�x���g�̑Ώۃv���C���[�E�p�[�}�l���g
        /// </summary>
        public MTGTarget Target { get; private set; }

        /// <summary>
        /// �C�x���g�̃A�N�V����
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
        /// �C�x���g�I�����̌��ʂ�z��ŕԂ��B
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