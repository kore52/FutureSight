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

        public MTGPlayChoiceResult GetPlayChoice()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �C�x���g�̑Ώۃv���C���[�E�p�[�}�l���g�E�J�[�h
        /// </summary>
        public List<MTGTarget> Targets { get; private set; }
        
        /// <summary>
        /// �C�x���g�̔�����
        /// </summary>
        public MTGSource Source { get; private set; }

        /// <summary>
        /// �C�x���g�̃A�N�V����
        /// </summary>
        public MTGEventAction Action { get; private set; }

        public MTGEvent() { }

        public MTGEvent(MTGSource source, MTGPlayer player, List<MTGTarget> targets, MTGChoice choice, MTGEventAction action)
        {
            Source = source;
            Player = player;
            Targets = targets;
            Choice = choice;
            Action = action;
            
            Console.WriteLine("AddEvent: " + this.GetType().ToString());
        }

        public MTGEvent(MTGPlayer player, List<MTGTarget> targets, MTGChoice choice, MTGEventAction action)
            : this(null, player, targets, choice, action) {}

        public MTGEvent(MTGPlayer player, List<MTGTarget> targets, MTGEventAction action)
            : this(null, player, targets, null, action) {}

        public void Execute(MTGGame game, MTGChoiceResults choiceResults)
            => Action.ExecuteEvent(game, this);

        public bool HasChoice()
            => Choice.IsValid();

        /// <summary>
        /// �C�x���g�I�����̌��ʂ�z��ŕԂ��B
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public MTGChoiceResults GetChoiceResults(MTGGame game)
        {
            return Choice.GetChoiceResults(game, this);
        }
    }
    
}