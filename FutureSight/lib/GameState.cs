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
        public List<MTGPlayer> Players { get; set; }
        public LinkedList<string> Stack { get; set; }
        public int Priority { get; set; }
        public MTGPhase Phase { get; set; }
        public MTGStep Step { get; set; }
        public int Turn { get; set; }
        public Move CurrentMove { get; set; }
        private LinkedList<MTGAction> actions;
        private LinkedList<MTGAction> delayedActions;
        public LinkedList<MTGEvent> Events { get; set; }
        
        public bool IsFinished { get { return false; } }
        public List<int> TurnOrder { get; set; }
        public MTGEvent GetNextEvent() { return eventQueue.First.Value; }
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

        private MTGPlayer scorePlayer;
        public MTGPlayer TurnPlayer { get; set; }
        private LinkedList<MTGEvent> eventQueue;
        
        // method
        
        public GameState()
        {
            Turn = 0;
            Score = 0;
            TurnOrder = new List<int>() { (int)PLAYER._0, (int)PLAYER._1 };
            Players = new List<MTGPlayer>();
            Stack = new LinkedList<string>();

            Priority = (int)PLAYER._0;
            
            Initialize();
        }

        public void Initialize()
        {
            // �v���C���[�̓ǂݍ���
            this.Players.Add(new MTGPlayer());
            this.Players.Add(new MTGPlayer());


            // ���C�u�����[�V���b�t��
            foreach (var p in this.Players)
            {
                Utilities.Shuffle(p.Library);
            }

            // �V���h���[
            foreach (var p in this.Players)
            {
                p.DrawCard();
                p.DrawCard();
                p.DrawCard();
                p.DrawCard();
                p.DrawCard();
                p.DrawCard();
                p.DrawCard();
            }
#if DEBUG
            System.Diagnostics.Debug.Print(String.Format("game initialization is ok"));
#endif
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
        private bool SetStateCheckRequired { set { stateCheckFlag; } }
        public bool IsStateCheckRequired { get { return stateCheckFlag; } }
        public void CheckStatePutTriggers()
        {
            // �󋵋N�������̕K�v���Ȃ��Ȃ�܂ŌJ��Ԃ�
            while (IsStateCheckRequired)
            {
                IsStateCheckRequired = false;

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

        // �t�F�C�Y�����s
        public void ExecutePhase()
        {
            Phase.ExecutePhase(this);
        }

        // �A�N�V������o�^
        public void AddAction(MTGAction action)
        {
            actions.AddLast(action);
        }

        // �A�N�V���������s
        public void DoAction(MTGAction action)
        {
            // �s�����A�N�V�������L�^
            actions.AddLast(action);

            // �n���ꂽ�A�N�V�����I�u�W�F�N�g���ɈقȂ�A�N�V���������s
            action.DoAction(this);

            // �A�N�V�����X�R�A�����Z
            Score += action.GetScore(this.scorePlayer);
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

        // �C�x���g�����s
        //  chioceResults: �I����������ꍇ�̌���
        public void ExecuteEvent(MTGEvent mtgevent, MTGChoiceResults choiceResults)
        {
            System.Diagnostics.Debug.Assert(choiceResults != null);

            // �n���ꂽ�C�x���g�I�u�W�F�N�g���ɈقȂ�C�x���g�����s
            mtgevent.Execute(this, choiceResults);
        }

        // ���̃C�x���g�����s
        // �������ׂ��C�x���g�������Ȃ����i�K�ŌĂ΂�郁�\�b�h
        public void ExecuteNextEvent(MTGChoiceResults choiceResults)
        {
            DoAction(new ExecuteFirstEventAction(choiceResults));
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
            var game = new GameState();
            game.Players = players;
            game.TurnPlayer = startPlayer;
            return game;
        }
    }
}