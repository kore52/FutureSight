using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

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
        public GameState()
        {
            ElapsedTurns = 0;
            TurnOrder = new List<int>() { (int)PLAYER._0, (int)PLAYER._1 };
            canPlayLand = true;

            Players = new List<PlayerState>();
            Stack = new LinkedList<string>();

            Step = GamePhase.UntapStep;
            Priority = (int)PLAYER._0;
        }
/*
        public GameState(GameState s)
        {
            this.turns = s.turns;
            this.turnQueue = new List<int>(s.turnQueue);
            this.canPlayLand = s.canPlayLand;

            this.Players = new List<Player>(s.Players);
        }
        */
        public void Initialize()
        {
            // プレイヤーの読み込み
            this.Players.Add(new PlayerState());
            this.Players.Add(new PlayerState());


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

        public static void Calc(GameTree parent, int depth)
        {
            if (depth > (int)Depth.Max) { return; }

            if (parent == null)
            {
                parent = new GameTree();
            }

            // 優先権を持っているプレイヤーの次の行動の候補を探索
            var moveCandidates = parent.Data.Search();
            foreach (var move in moveCandidates)
            {
                // 探索した行動ごとに盤面を進める
                GameState stateAfterMove = DoMove(move, parent.Data);
                GameTree newTree = new GameTree(stateAfterMove);

                // 移動後の評価値を計算
                newTree.Score = Evaluate.evaluate(stateAfterMove);

                // 親ノードのスコアを更新
                parent.Score = Math.Max(parent.Score, newTree.Score);

                parent.Node.Add(newTree);

#if DEBUG
                // 木の状態を表示
                string sp = "";
                for (int c=0; c <depth; c++) { sp += " "; }
                System.Diagnostics.Debug.Print(String.Format(sp + "Depth{0}->{1}: ActPly:{2}, Pri:{3}, Turn:{4}, Step:{5}",
                    depth, depth + 1,
                    stateAfterMove.GetActivePlayerNumber(), 
                    stateAfterMove.Priority,
                    (int)stateAfterMove.ElapsedTurns,
                    (int)stateAfterMove.Step));

                // 盤面の状態を表示
                System.Diagnostics.Debug.Print(String.Format(sp + "[me:H:{0}, P:{1}], [op:H:{2}, P:{3}]",
                    Utilities.Join(stateAfterMove.Players[0].Hand),
                    String.Join(",", stateAfterMove.Players[0].Permanents.Select(item => item.ID).ToArray()),
                    Utilities.Join(stateAfterMove.Players[1].Hand),
                    String.Join(",", stateAfterMove.Players[1].Permanents.Select(item => item.ID).ToArray())));
#endif

                // 子ノードの行動探索
                Calc(newTree, depth + 1);
            }
        }

        public List<Move> Search()
        {
            var nextMove = new List<Move>();

            PlayerState player = Players[Priority];

            for (int i = 0; i < player.Hand.Count; i++)
            {
                Card item = CardDB.GetInstance().get(player.Hand[i]);
                switch (Step)
                {
                    case GamePhase.UntapStep: break;
                    case GamePhase.UpkeepStep:
                    case GamePhase.DrawStep:
                        if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        break;
                    case GamePhase.Main1:
                        if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }

                        if (player.ID == GetActivePlayer().ID)
                        {
                            if (item.CardType.HasFlag(CardType.Land) && canPlayLand) { nextMove.Add(GetActivePlayerNumber() + ":play:" + i); }
                            if (item.CardType.HasFlag(CardType.Creature) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Sorcery) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Enchantment) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Artifact) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Planeswalker) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        }
                        break;
                    case GamePhase.PreCombatStep:
                    case GamePhase.DeclareAttackerStep:
                    case GamePhase.DeclareBlockerStep:
                    case GamePhase.CombatDamageStep:
                    case GamePhase.EndOfCombatStep:
                        if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        break;
                    case GamePhase.Main2:
                        if (item.CardType.HasFlag(CardType.Land) && canPlayLand) { nextMove.Add(GetActivePlayerNumber() + ":play:" + i); }

                        if (player.ID == GetActivePlayer().ID)
                        {
                            if (item.CardType.HasFlag(CardType.Creature) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Sorcery) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Enchantment) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Artifact) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                            if (item.CardType.HasFlag(CardType.Planeswalker) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        }
                        break;
                    case GamePhase.EndStep:
                        if (item.CardType.HasFlag(CardType.Instant) && IsManaCostSatisfied(item.ManaCost, player)) { nextMove.Add(GetActivePlayerNumber() + ":cast:" + i); }
                        break;
                    case GamePhase.CleanupStep: break;

                }

            }
            nextMove.Add(Priority + ":none");

#if DEBUG
            string c = "";
            foreach (var nm in nextMove) { c += "[" + nm + "]"; }
            System.Diagnostics.Debug.Print(c);
#endif
            return nextMove;
        }

        // 1手進めた盤面を返す
        public static GameState DoMove(Move move, GameState previousState)
        {
            // 盤面情報のコピー
            GameState state = (GameState)previousState.DeepCopy();

            string[] moveElement = move.Split(':');
            int indexOfPlayer = int.Parse(moveElement[0]);

            // ターン起因処理
            
            switch (moveElement[1])
            {
                // 優先権をパスし、次のプレイヤーに優先権を渡す
                case "none":
                    if (state.TurnOrder.Count > state.Priority + 1)
                    {
                        state.Priority++;
                    } else {
                        state.Priority = state.TurnOrder[0];
                        if (state.Step != GamePhase.CleanupStep)
                        {
                            state.Step++;
                        }
                        else {
                            state.Step = GamePhase.UntapStep;
                            state.ElapsedTurns++;
                        }
                    }
                    break;

                // 土地のプレイ
                case "play":
                    int indexOfHand = int.Parse(moveElement[2]);
                    // パーマネントに登録し、手札から除外する
                    int hid = state.Players[indexOfPlayer].Hand[indexOfHand];
                    state.Players[indexOfPlayer].Permanents.Add(new Permanent(indexOfHand));
                    state.Players[indexOfPlayer].Hand.RemoveAt(hid);
                    break;
            }

            // 行った動作を保存
            state.CurrentMove = move;
            CheckStateBasedAction(state);


            return state;
        }

        // ターン起因処理
        public static void TurnBasedAction(GameState state)
        {
            foreach (var p in state.Players)
            {
                switch(state.Step)
                {
                    case GamePhase.UntapStep: break;
                    case GamePhase.DrawStep: break;
                    case GamePhase.DeclareAttackerStep: break;
                    case GamePhase.DeclareBlockerStep: break;
                    case GamePhase.CombatDamageStep: break;
                    case GamePhase.CleanupStep: break;
                    default: break;
                }
            }
        }

        // 状況起因処理
        public static void CheckStateBasedAction(GameState state)
        {
            foreach (var p in state.Players)
            {
                // ライフが0以下のプレイヤーはゲームに敗北する
                if (p.Life <= 0) { p.IsLose = true; }

                // 毒カウンターが10個以上のプレイヤーはゲームに敗北する
                if (p.Poison >= 10) { p.IsLose = true; }

                // ライブラリーが空の状態でドローしたプレイヤーはゲームに敗北する
                if (p.IsEmptyDraw) { p.IsLose = true; }
            }


            // ゲームの終了判定
            //   ・1人以上が勝利する
            int cntWin = state.Players.Where(p => p.IsWin == true).Count();
            if (cntWin > 0) { state.isGameFinished = true; return; }

            //   ・1人を残してその他全員が敗北する
            int cntLose = state.Players.Where(p => p.IsLose == true).Count();
            if (cntLose == state.Players.Count - 1) { state.isGameFinished = true; return; }
        }

        // misc
        public int GetActivePlayerNumber() { return TurnOrder.First(); }
        public PlayerState GetActivePlayer() { return Players[TurnOrder.First()]; }

        public List<PlayerState> Players { get; set; }
        public LinkedList<string> Stack { get; set; }

        public void ForwardTurn() { ElapsedTurns++; }
        public int GetElapsedTurn() { return ElapsedTurns; }

        public int Priority { get; set; }
        public GamePhase Step { get; set; }
        public int ElapsedTurns { get; set; }

        public Move CurrentMove { get; set; }
        public bool IsGameFinished { get { return isGameFinished; } }

        public List<int> TurnOrder { get; set; }

        private LinkedList<MTGEvent> eventQueue;
        public MTGEvent GetNextEvent() { return eventQueue.First.Value; }




        // 内部

        private bool canPlayLand;
        private bool isGameFinished;

        public List<GameState> MoveNode { get; set; }

        private bool IsTargetExists()
        {
            return false;
        }

        private bool IsManaCostSatisfied(string cost, PlayerState player)
        {
            bool result = false;

            // calc max mana in manapool
            int max = player.ManaPool.Sum();
            List<int> costList = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
            List<int> tmp = new List<int>(player.ManaPool);

            var match = cost.Split('{');
            foreach (var m in match)
            {
                var ms = m;
                if (m.Length <= 0) { continue; } else { ms = m.Substring(0, m.Length - 1); }

                switch (ms)
                {
                    case "W": costList[(int)Color.White]++; break;
                    case "U": costList[(int)Color.Blue]++; break;
                    case "B": costList[(int)Color.Black]++; break;
                    case "R": costList[(int)Color.Red]++; break;
                    case "G": costList[(int)Color.Green]++; break;
                    case "C": costList[(int)Color.Colorless]++; break;
                    case "X":
                    case "Y":
                    case "Z":
                        break;
                    default:
                        // 不特定マナ
                        costList[(int)Color.Generic] += int.Parse(ms);
                        break;
                }
            }

            tmp[0] = (tmp[0] - costList[0] >= 0) ? tmp[0] - costList[0] : 0;
            tmp[1] = (tmp[1] - costList[1] >= 0) ? tmp[1] - costList[1] : 0;
            tmp[2] = (tmp[2] - costList[2] >= 0) ? tmp[2] - costList[2] : 0;
            tmp[3] = (tmp[3] - costList[3] >= 0) ? tmp[3] - costList[3] : 0;
            tmp[4] = (tmp[4] - costList[4] >= 0) ? tmp[4] - costList[4] : 0;
            tmp[5] = (tmp[5] - costList[5] >= 0) ? tmp[5] - costList[5] : 0;

            if (costList[(int)Color.White] <= player.ManaPool[(int)Color.White] &&
                costList[(int)Color.Blue] <= player.ManaPool[(int)Color.Blue] &&
                costList[(int)Color.Black] <= player.ManaPool[(int)Color.Black] &&
                costList[(int)Color.Red] <= player.ManaPool[(int)Color.Red] &&
                costList[(int)Color.Green] <= player.ManaPool[(int)Color.Green] &&
                costList[(int)Color.Colorless] <= player.ManaPool[(int)Color.Colorless] &&
                costList[(int)Color.Generic] <= tmp.Sum())
            {
                result = true;
            }
            return result;
        }
    }

    //-------------------------------------------------------------
    // ディープコピー機能を持った拡張メソッド
    //-------------------------------------------------------------
    static class Utilities
    {
        public static object DeepCopy(this object target)
        {
            object result;
            BinaryFormatter b = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = b.Deserialize(mem);
#if NULL
                System.Diagnostics.Debug.Print("CopyEnd, Size(bytes):{0}", mem.Length);
#endif
            }
            finally
            {
                mem.Close();
            }
            return result;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string Join(this List<int> list)
        {
            return string.Join(",", list.Select(item => item.ToString()).ToArray());
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