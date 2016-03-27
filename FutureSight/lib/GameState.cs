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

        public GameState()
        {
            Turn = 0;
            TurnOrder = new List<int>() { (int)PLAYER._0, (int)PLAYER._1 };

            Players = new List<MTGPlayer>();
            Stack = new LinkedList<string>();

            Priority = (int)PLAYER._0;
        }

        public void Initialize()
        {
            // プレイヤーの読み込み
            this.Players.Add(new MTGPlayer());
            this.Players.Add(new MTGPlayer());


            // ライブラリーシャッフル
            foreach (var p in this.Players)
            {
                Utilities.Shuffle(p.Library);
            }

            // ７枚ドロー
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


        // ゲームの状態を更新する
        public void Update()
        {
            DoDelayedAction();

        }

        // 状況起因処理
        public bool IsStateCheckRequired { get; set; }
        public void CheckStatePutTriggers()
        {
            // チェックの必要がなくなるまで繰り返す
            while (IsStateCheckRequired)
            {
                IsStateCheckRequired = false;

                // プレイヤーの敗北チェック
                foreach (var player in GetAPNAP())
                {
                    // 敗北していれば敗北アクションを生成
                    player.GenerateStateBasedActions();
                }

                // パーマネントの状態チェック
                foreach (var player in GetAPNAP())
                {
                    foreach (var permanent in player.Permanents)
                    {
                        permanent.GenerateStateBasedActions();
                    }
                }

                // ゲームの状態を更新
                Update();
            }
        }

        // ゲームを巻き戻す
        public void Restore()
        {
            MTGAction action = actions.Last();
            action.UndoAction(this);
        }

        // フェイズを実行
        public void ExecutePhase()
        {
            Phase.ExecutePhase(this);
        }

        // アクションを登録
        public void AddAction(MTGAction action)
        {
            actions.AddLast(action);
        }

        // アクションを実行
        public void DoAction(MTGAction action)
        {
            // 行ったアクションを記録
            actions.AddLast(action);

            // アクションを実行
            action.DoAction(this);

            // アクションスコアを加算
            Score += action.GetScore(this.scorePlayer);            
        }

        // 遅延アクションを登録
        public void AddDelayedAction(MTGAction action)
        {
            delayedActions.AddLast(action);
        }

        // 遅延アクションを実行
        public void DoDelayedAction()
        {
            while (delayedActions.Count != 0)
            {
                var action = delayedActions.First();
                delayedActions.RemoveFirst();
                DoAction(action);
            }
        }

        // イベントを実行
        public void ExecuteEvent(MTGEvent mtgevent, MTGChoiceResults choiceResults)
        {
            System.Diagnostics.Debug.Assert(choiceResults != null);

            mtgevent.Execute(this, choiceResults);
        }

        public void ExecuteNextEvent(MTGChoiceResults choiceResults)
        {
            DoAction(new ExecuteFirstEventAction(choiceResults));
        }

        // プレイヤーリストをAPNAP順で取得
        public List<MTGPlayer> GetAPNAP()
        {
            var ret = new List<MTGPlayer>() { TurnPlayer };
            ret.AddRange(TurnPlayer.Opponents);
            return ret;
        }
    }

    public class GameTree
    {
        public GameTree ()
        {
            Node = new List<GameTree>();
        }
        public GameTree (GameTree t)
        {
            Node = new List<GameTree>(t.Node);
            Data = (GameState)Utilities.DeepCopy(t.Data);
        }
        public GameTree (GameState s)
        {
            Node = new List<GameTree>();
            Data = s;
        }
        public List<GameTree> Node { get; set; }
        public GameState Data { get; set; }
        public int Score { get; set; }
    }
}