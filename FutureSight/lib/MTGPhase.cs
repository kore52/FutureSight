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
        public abstract MTGPhase GetInstance();
        public abstract void ExecuteBeginPhase(GameState game);
        public abstract MTGPhaseType GetType();
    }
    
    // ステップ固有処理
    public class MTGUntapStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGUntapStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.Untap; }
    }
    
    public class MTGUpkeepStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGUpkeepStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.Upkeep; }
    }
    
    public class MTGDrawStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGDrawStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.Draw; }
    }

    public class MTGFirstMainPhase : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGFirstMainPhase(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.FirstMain; }
    }

    public class MTGPreCombatStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGPreCombatStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.PreCombat; }
    }

    public class MTGDeclareAttackerStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGDeclareAttackerStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.DeclareAttacker; }
    }

    public class MTGDeclareBlockerStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGDeclareBlockerStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.DeclareBlocker; }
    }

    public class MTGDamageStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGDamageStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        private override string step = "Damage";
    }

    public class MTGEndCombatStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGEndCombatStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.EndCombat; }
    }

    public class MTGSecondMainPhase : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGSecondMainPhase(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.SecondMain; }
    }

    public class MTGEndStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGEndStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.End; }
    }

    public class MTGCleanupStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public override MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGCleanupStep(); }
            return instance;
        }
        
        public override void ExecuteBeginPhase(GameState game)
        {
        }
        
        public override MTGPhaseType GetType() { return MTGPhaseType.Cleanup; }
    }
    
    public class MTGGamePlayProgression
    {
        public static MTGPhase NextPhase(GameState game)
        {
            switch (game.Phase)
            {
            case Untap:
                return MTG
                break;
            case Upkeep:
                break;
            case Draw:
                break;
            case FirstMain:
                break;
            case PreCombat:
                break;
            case DeclareAttacker:
                break;
            case DeclareBlocker:
                break;
            case Damage:
                break;
            case EndCombat:
                break;
            case SecondMain:
                break;
            case End:
                break;
            case Cleanup:
                break;
            default:
                throw Exception("Invalid step");
            }
        }
    }
}