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
    
    public class MTGPhase
    {
        public string Step { get { return step; } }
        protected string step;

        public void ExecuteBeginPhase(GameState game) { }

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
    }
    
    // ステップ固有処理
    public class MTGUntapStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? (instance = new MTGUntapStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "Untap";
    }
    
    public class MTGUpkeepStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGUpkeepStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "Upkeep";
    }
    
    public class MTGDrawStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGDrawStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "Draw";
    }

    public class MTGFirstMainPhase : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGFirstMainPhase()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "FirstMain";
    }

    public class MTGPreCombatStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGPreCombatStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "PreCombat";
    }

    public class MTGDeclareAttackerStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGDeclareAttackerStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "DeclareAttacker";
    }

    public class MTGDeclareBlockerStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGDeclareBlockerStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "DeclareBlocker";
    }

    public class MTGDamageStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGDamageStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "Damage";
    }

    public class MTGEndCombatStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGEndCombatStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "EndCombat";
    }

    public class MTGSecondMainPhase : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGSecondMainPhase()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "SecondMain";
    }

    public class MTGEndStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGEndStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "End";
    }

    public class MTGCleanupStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? (instance = new MTGCleanupStep()); }
        
        public new void ExecuteBeginPhase(GameState game)
        {
        }
        
        private new string step = "Cleanup";
    }
}