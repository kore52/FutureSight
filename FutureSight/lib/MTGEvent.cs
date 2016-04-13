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
        public MTGPermanent Permanent { get { return (MTGPermanent)Source; } }

        /// <summary>
        /// �C�x���g�̃A�N�V����
        /// </summary>
        public MTGEventAction Action { get; private set; }
        
        /// <summary>
        /// �C�x���g�Ŏ�蓾��I�����ꗗ
        /// �C�x���g���s���͒l��ێ�����(�C�x���g���I����null�N���A)
        ///   ��F�Ώۂ̃N���[�`���[�̏ꍇ -> �N���[�`���[�ꗗ�����X�g�ɑ��݂���
        /// </summary>
        public MTGPlayChoiceResult Chosen { get; private set; }
        public MTGTarget ChosenTarget { get; private set; }
        
        public string Descriptions;
        
        public MTGEvent() { }
        public MTGEvent(MTGSource source, MTGPlayer player, List<MTGTarget> targets, MTGChoice choice, MTGEventAction action, string descriptions)
        {
            Source = source;
            Player = player;
            Targets = targets;
            Choice = choice;
            Action = action;
            Descriptions = descriptions;
            
            Console.WriteLine("AddEvent: " + this.GetType().ToString());
        }
        public MTGEvent(MTGPlayer player, List<MTGTarget> targets, MTGChoice choice, MTGEventAction action, string descriptions)
            : this(null, player, targets, choice, action, descriptions) {}
        public MTGEvent(MTGSource source, MTGPlayer player, List<MTGTarget> targets, MTGEventAction action, string descriptions)
            : this(source, player, targets, null, action, descriptions) {}
        public MTGEvent(MTGPlayer player, List<MTGTarget> targets, MTGEventAction action, string descriptions)
            : this(null, player, targets, null, action, descriptions) {}
        public MTGEvent(MTGSource source, MTGEventAction action, string descriptions)
            : this(source, source.Controller, null, null, action, descriptions) {}

        public void ExecuteEvent(MTGGame game, MTGChoiceResults choiceResults)
        {
            Chosen = choiceResults;
            ChosenTarget = GetLegalTarget(game, Chosen);
            Action.ExecuteEvent(game, this); // delegation
            Chosen = null;
            ChosenTarget = null;
        }

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
        
        private MTGTarget GetLegalTarget(MTGGame game, MTGChoiceResults chosen)
        {
            foreach (var target in chosen)
                if (target is MTGTarget)
                    return GetLegalTarget(game, target);
            return null;
        }
        
        private MTGTarget GetLegalTarget(MTGGame game, MTGTarget target)
        {
            return target;
        }
    }
    
}