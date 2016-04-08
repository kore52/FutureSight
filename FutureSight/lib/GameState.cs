using System;
using System.Collections.Generic;
using System.Linq;

using Move = System.String;

namespace FutureSight.lib
{
    enum PLAYER
    {
        _0 = 0,
        _1,
        _2,
        _3
    }

    enum Depth : int
    {
        None = -1,
        Zero = 0,
        Max = 6
    }
    enum BehaviorCode
    {
        PutLand,
        CastSpell
    }

    public enum GamePhase
    {
        UntapStep,
        UpkeepStep,
        DrawStep,
        Main1,
        PreCombatStep,
        DeclareAttackerStep,
        DeclareBlockerStep,
        CombatDamageStep,
        EndOfCombatStep,
        Main2,
        EndStep,
        CleanupStep
    }

    [Serializable()]
    public class GameState
    {
        /// <summary>
        /// �v���C���[
        /// </summary>
        public List<MTGPlayer> Players { get; private set; }

        /// <summary>
        /// �̈�:�X�^�b�N
        /// </summary>
        public LinkedList<string> Stack { get; set; }

        /// <summary>
        /// �D�挠�����v���C���[ID
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// ���݂̃t�F�C�Y�E�X�e�b�v
        /// </summary>
        public MTGPhase Phase { get; set; }

        /// <summary>
        /// �t�F�C�Y�E�X�e�b�v���̏�Ԃ����i�A�N�e�B�u�v���C���[�A�������Ȃǁj
        /// </summary>
        public MTGStep Step { get; set; }

        public int Turn { get; set; }

        private LinkedList<MTGAction> actions;
        private LinkedList<MTGAction> delayedActions;

        public MTGEventQueue Events { get; set; }
        
        public List<int> TurnOrder { get; set; }

        /// <summary>
        /// ���݂̔Ֆʂ̐ÓI�]���l
        /// </summary>
        public int Score { get; set; }
        public long ID
        {
            get
            {
                long id = 0;
                id += Turn * 1000000000000;
                id += (long)Step * 1000000000;
                id += Players.Sum(p => p.ID);
                return id;
            }
        }
        public List<GameState> MoveNode { get; set; }

        // �X�R�A��]������v���C���[
        public MTGPlayer ScorePlayer { get; set; }

        // �^�[�����s���v���C���[
        public MTGPlayer TurnPlayer { get; set; }
        
        // �s�k�����v���C���[
        public MTGPlayer LosingPlayer { get; set; }
        
        
        
        public GameState()
        {
            Turn = 1;
            Score = 0;
            TurnOrder = new List<int>() { (int)PLAYER._0, (int)PLAYER._1 };
            Players = new List<MTGPlayer>();
            Stack = new LinkedList<string>();
            Priority = (int)PLAYER._0;
            Events = new MTGEventQueue();
            actions = new LinkedList<MTGAction>();
            delayedActions = new LinkedList<MTGAction>();
            ChangePhase(MTGDefaultGamePlay.GetInstance().GetStartPhase(this));
        }

        public GameState(List<MTGPlayer> players, MTGPlayer startPlayer) : this()
        {
            Players = players;
            ScorePlayer = startPlayer;
            TurnPlayer = startPlayer;
        }

        // misc
        public int GetActivePlayerNumber()
        {
            return TurnOrder.First();
        }

        public MTGPlayer GetActivePlayer()
        {
            return Players[TurnOrder.First()];
        }

        public void ForwardTurn()
        {
            Turn++;
        }

        // �Q�[���̏�Ԃ��X�V����
        public void Update()
        {
            DoDelayedAction();
        }

        // �󋵋N������
        private bool stateCheckFlag = false;
        private void SetStateCheckRequired(bool flag) { stateCheckFlag = flag; }
        public void SetStateCheckRequired() { stateCheckFlag = true; }
        public bool IsStateCheckRequired { get { return stateCheckFlag; } }
        
        public void CheckStatePutTriggers()
        {
            // �󋵋N�������̕K�v���Ȃ��Ȃ�܂ŌJ��Ԃ�
            while (IsStateCheckRequired)
            {
                stateCheckFlag = false;

                // �v���C���[�̔s�k�`�F�b�N
                foreach (var player in GetAPNAP())
                {
                    // �s�k���Ă���Δs�k�A�N�V�����𐶐�
                    player.GenerateStateBasedActions();
                }

                // �p�[�}�l���g�̏�ԃ`�F�b�N
                // �^�t�l�X0�ł̕�n�ړ��A�v���_���[�W�ł̔j��A�I�[���A+1/+1,-1/-1�J�E���^�[�̑��E�Ȃǂ��s��
                foreach (var player in GetAPNAP())
                {
                    foreach (var permanent in player.Permanents)
                    {
                        permanent.GenerateStateBasedActions();
                    }
                }

                // �Q�[���̏�Ԃ��X�V
                Update();
            }
        }

        // �Q�[���������߂�
        public void Restore()
        {
            MTGAction action = actions.Last();
            action.UndoAction(this);
        }

        // ���̃X�e�b�v�E�t�F�C�Y�Ɉڍs
        public void NextPhase()
        {
            ChangePhase(MTGDefaultGamePlay.GetInstance().NextPhase(this));
        }

        // �X�e�b�v�E�t�F�C�Y�̈ړ�
        private void ChangePhase(MTGPhase phase)
        {
            Phase = phase;
            Step = MTGStep.Begin;
        }

        // �t�F�C�Y�����s
        public void ExecutePhase()
        {
            Phase.ExecutePhase(this);
        }

        public bool IsFinished()
            => LosingPlayer != null;
        
        public bool CanSkipPhase()
            => Stack.Count == 0;
        
        // �A�N�V������o�^
        public void AddAction(MTGAction action)
        {
            actions.AddLast(action);
        }

        // �A�N�V���������s
        public void DoAction(MTGAction action)
        {
            Log(ScorePlayer, action.GetType().ToString());
            
            // �s�����A�N�V�������L�^
            actions.AddLast(action);

            // �n���ꂽ�A�N�V�����I�u�W�F�N�g���ɈقȂ�A�N�V���������s
            action.DoAction(this);

            // �A�N�V�����X�R�A�����Z
            Score += action.GetScore(this.ScorePlayer);
        }

        // �x���A�N�V������o�^
        public void AddDelayedAction(MTGAction action)
        {
            delayedActions.AddLast(action);
        }

        // �x���A�N�V���������s
        public void DoDelayedAction()
        {
            while (delayedActions.Count != 0)
            {
                var action = delayedActions.First();
                delayedActions.RemoveFirst();
                DoAction(action);
            }
        }

        // �I�����������̃C�x���g�܂ŃQ�[����i�߂�
        public bool AdvanceToNextEventWithChoice()
        {
            while (!IsFinished())
            {
                if (!HasNextEvent())
                    // �C�x���g��������Ύ��̃X�e�b�v�ɐi�߂�
                    ExecutePhase();
                else if (!GetNextEvent().HasChoice())
                    // �C�x���g�͂����Ă��I�������Ȃ���Ύ��̃C�x���g����������
                    ExecuteNextEvent();
                else
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// �C�x���g�̒ǉ�
        /// </summary>
        public void AddEvent(MTGEvent aEvent)
            => DoAction(new AddEventAction(aEvent));
        
        // �C�x���g�����s
        //  chioceResults: �I����������ꍇ�̌���
        public void ExecuteEvent(MTGEvent aEvent, MTGChoiceResults choiceResults)
        {
            System.Diagnostics.Debug.Assert(choiceResults != null);

            // �n���ꂽ�C�x���g�I�u�W�F�N�g���ɈقȂ�C�x���g�����s
            aEvent.Execute(this, choiceResults);
        }
        
        // �C�x���g�L���[�Ɏ��̃C�x���g�����邩
        public bool HasNextEvent()
            => Events.Count != 0;
        
        // ���̃C�x���g�����s
        // �������ׂ��C�x���g�������Ȃ����i�K�ŌĂ΂�郁�\�b�h
        public void ExecuteNextEvent(MTGChoiceResults choiceResults)
        {
            DoAction(new ExecuteFirstEventAction(choiceResults));
        }
        
        // ���̃C�x���g�����s
        // �I�����������o�[�W����
        public void ExecuteNextEvent()
        {
            DoAction(new ExecuteFirstEventAction(null));
        }
        
        // �C�x���g�L���[�̐擪�̃C�x���g���擾
        public MTGEvent GetNextEvent()
        {
            
            return Events.First;
        }
        
        // �v���C���[���X�g��APNAP���Ŏ擾
        public List<MTGPlayer> GetAPNAP()
        {
            var ret = new List<MTGPlayer>() { TurnPlayer };
            ret.AddRange(TurnPlayer.Opponents);
            return ret;
        }
        
        public static GameState CreateGame(List<MTGPlayer> players, MTGPlayer startPlayer)
        {
            var game = new GameState(players, startPlayer);
            return game;
        }
        
        public void Log(MTGPlayer player, string message)
        {
            Console.WriteLine("[" + player.Name + "] " + message);
        }
    }
}