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
    public enum MTGStep
    {
        Begin,
        ActivePlayer,
        OtherPlayer,
        Resolve,
        NextPhase,
    }

    public enum MTGPhaseType
    {
        Null,
        Mulligan,
        Untap,
        Upkeep,
        Draw,
        FirstMain,
        PreCombat,
        DeclareAttacker,
        DeclareBlocker,
        Damage,
        EndCombat,
        SecondMain,
        End,
        Cleanup
    }
    
    public abstract class MTGPhase
    {
        public void ExecutePhase(GameState game)
        {
            switch (game.Step)
            {
                case MTGStep.Begin:
                    // ステップ固有処理の実行
                    ExecuteBeginPhase(game);
                    game.Update();
                    break;
                case MTGStep.ActivePlayer:
                    // 状況起因処理の実行
                    game.CheckStatePutTriggers();

                    break;
                case MTGStep.OtherPlayer:
                    break;
                case MTGStep.Resolve:
                    break;
                case MTGStep.NextPhase:
                    break;
            }
        }
        public abstract void ExecuteBeginPhase(GameState game);
        public MTGPhaseType Type { get { return MTGPhaseType.Null; } }
    }
    
    // ステップ固有処理
    public class MTGUntapStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGUntapStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.Untap; } }
    }
    
    public class MTGUpkeepStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGUpkeepStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.Upkeep; } }
    }
    
    public class MTGDrawStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGDrawStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.Draw; } }
    }

    public class MTGFirstMainPhase : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGFirstMainPhase(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.FirstMain; } }
    }

    public class MTGPreCombatStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGPreCombatStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.PreCombat; } }
    }

    public class MTGDeclareAttackerStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGDeclareAttackerStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.DeclareAttacker; } }
    }

    public class MTGDeclareBlockerStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGDeclareBlockerStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.DeclareBlocker; } }
    }

    public class MTGDamageStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGDamageStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }

        public new MTGPhaseType Type { get { return MTGPhaseType.Damage; } }

    }

    public class MTGEndCombatStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGEndCombatStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.EndCombat; } }
    }

    public class MTGSecondMainPhase : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGSecondMainPhase(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.SecondMain; } }
    }

    public class MTGEndStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGEndStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.End; } }
    }

    public class MTGCleanupStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGCleanupStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public new MTGPhaseType Type { get { return MTGPhaseType.Cleanup; } }
    }
    
    public class MTGGamePlayProgression
    {
        public static MTGPhase NextPhase(GameState game)
        {
            switch (game.Phase.Type)
            {
                case MTGPhaseType.Untap:
                    return MTGUntapStep.GetInstance();
                case MTGPhaseType.Upkeep:
                    return MTGUpkeepStep.GetInstance();
                case MTGPhaseType.Draw:
                    return MTGDrawStep.GetInstance();
                case MTGPhaseType.FirstMain:
                    return MTGFirstMainPhase.GetInstance();
                case MTGPhaseType.PreCombat:
                    return MTGFirstMainPhase.GetInstance();
                case MTGPhaseType.DeclareAttacker:
                    return MTGDeclareAttackerStep.GetInstance();
                case MTGPhaseType.DeclareBlocker:
                    return MTGDeclareBlockerStep.GetInstance();
                case MTGPhaseType.Damage:
                    return MTGDamageStep.GetInstance();
                case MTGPhaseType.EndCombat:
                    return MTGEndCombatStep.GetInstance();
                case MTGPhaseType.SecondMain:
                    return MTGSecondMainPhase.GetInstance();
                case MTGPhaseType.End:
                    return MTGEndStep.GetInstance();
                case MTGPhaseType.Cleanup:
                    return MTGCleanupStep.GetInstance();
            }
            return null;
        }
    }
}