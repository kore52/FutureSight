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
        /// プレイヤー
        /// </summary>
        public List<MTGPlayer> Players { get; private set; }

        /// <summary>
        /// 領域:スタック
        /// </summary>
        public LinkedList<string> Stack { get; set; }

        /// <summary>
        /// 優先権を持つプレイヤーID
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 現在のフェイズ・ステップ
        /// </summary>
        public MTGPhase Phase { get; set; }

        /// <summary>
        /// フェイズ・ステップ中の状態を持つ（アクティブプレイヤー、解決中など）
        /// </summary>
        public MTGStep Step { get; set; }

        public int Turn { get; set; }

        private LinkedList<MTGAction> actions;
        private LinkedList<MTGAction> delayedActions;

        public MTGEventQueue Events { get; set; }
        
        public List<int> TurnOrder { get; set; }

        /// <summary>
        /// 現在の盤面の静的評価値
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

        // スコアを評価するプレイヤー
        public MTGPlayer ScorePlayer { get; set; }

        // ターンを行うプレイヤー
        public MTGPlayer TurnPlayer { get; set; }
        
        // 敗北したプレイヤー
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

        // ゲームの状態を更新する
        public void Update()
        {
            DoDelayedAction();
        }

        // 状況起因処理
        private bool stateCheckFlag = false;
        private void SetStateCheckRequired(bool flag) { stateCheckFlag = flag; }
        public void SetStateCheckRequired() { stateCheckFlag = true; }
        public bool IsStateCheckRequired { get { return stateCheckFlag; } }
        
        public void CheckStatePutTriggers()
        {
            // 状況起因処理の必要がなくなるまで繰り返す
            while (IsStateCheckRequired)
            {
                stateCheckFlag = false;

                // プレイヤーの敗北チェック
                foreach (var player in GetAPNAP())
                {
                    // 敗北していれば敗北アクションを生成
                    player.GenerateStateBasedActions();
                }

                // パーマネントの状態チェック
                // タフネス0での墓地移動、致死ダメージでの破壊、オーラ、+1/+1,-1/-1カウンターの相殺などを行う
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

        // 次のステップ・フェイズに移行
        public void NextPhase()
        {
            ChangePhase(MTGDefaultGamePlay.GetInstance().NextPhase(this));
        }

        // ステップ・フェイズの移動
        private void ChangePhase(MTGPhase phase)
        {
            Phase = phase;
            Step = MTGStep.Begin;
        }

        // フェイズを実行
        public void ExecutePhase()
        {
            Phase.ExecutePhase(this);
        }

        public bool IsFinished()
            => LosingPlayer != null;
        
        public bool CanSkipPhase()
            => Stack.Count == 0;
        
        // アクションを登録
        public void AddAction(MTGAction action)
        {
            actions.AddLast(action);
        }

        // アクションを実行
        public void DoAction(MTGAction action)
        {
            Log(ScorePlayer, action.GetType().ToString());
            
            // 行ったアクションを記録
            actions.AddLast(action);

            // 渡されたアクションオブジェクト毎に異なるアクションを実行
            action.DoAction(this);

            // アクションスコアを加算
            Score += action.GetScore(this.ScorePlayer);
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

        // 選択が伴う次のイベントまでゲームを進める
        public bool AdvanceToNextEventWithChoice()
        {
            while (!IsFinished())
            {
                if (!HasNextEvent())
                    // イベントが無ければ次のステップに進める
                    ExecutePhase();
                else if (!GetNextEvent().HasChoice())
                    // イベントはあっても選択肢がなければ次のイベントを処理する
                    ExecuteNextEvent();
                else
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// イベントの追加
        /// </summary>
        public void AddEvent(MTGEvent aEvent)
            => DoAction(new AddEventAction(aEvent));
        
        // イベントを実行
        //  chioceResults: 選択肢がある場合の結果
        public void ExecuteEvent(MTGEvent aEvent, MTGChoiceResults choiceResults)
        {
            System.Diagnostics.Debug.Assert(choiceResults != null);

            // 渡されたイベントオブジェクト毎に異なるイベントを実行
            aEvent.Execute(this, choiceResults);
        }
        
        // イベントキューに次のイベントがあるか
        public bool HasNextEvent()
            => Events.Count != 0;
        
        // 次のイベントを実行
        // 処理すべきイベントが無くなった段階で呼ばれるメソッド
        public void ExecuteNextEvent(MTGChoiceResults choiceResults)
        {
            DoAction(new ExecuteFirstEventAction(choiceResults));
        }
        
        // 次のイベントを実行
        // 選択肢が無いバージョン
        public void ExecuteNextEvent()
        {
            DoAction(new ExecuteFirstEventAction(null));
        }
        
        // イベントキューの先頭のイベントを取得
        public MTGEvent GetNextEvent()
        {
            
            return Events.First;
        }
        
        // プレイヤーリストをAPNAP順で取得
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