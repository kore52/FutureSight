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
    enum MTGPhaseType
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
        protected void ExecutePhase(GameState game) { }
        public string Step { get { return step; } }
        protected string step;
    }
    
    // ステップ固有処理
    public class MTGUntapStep : MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGUntapStep(); }
        
        public new void ExecutePhase(GameState game)
        {
        }
        
        private new string step = "Untap";
    }
    
    public class MTGUpkeepStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGUpkeepStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "Upkeep";
    }
    
    public class MTGDrawStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGDrawStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "Draw";
    }

    public class MTGFirstMainPhase : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGFirstMainPhase(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "FirstMain";
    }

    public class MTGPreCombatStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGPreCombatStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "PreCombat";
    }

    public class MTGDeclareAttackerStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGDeclareAttackerStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "DeclareAttacker";
    }

    public class MTGDeclareBlockerStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGDeclareBlockerStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "DeclareBlocker";
    }

    public class MTGDamageStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGDamageStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "Damage";
    }

    public class MTGEndCombatStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGEndCombatStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "EndCombat";
    }

    public class MTGSecondMainPhase : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGSecondMainPhase(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "SecondMain";
    }

    public class MTGEndStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGEndStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "End";
    }

    public class MTGCleanupStep : MTGPhase
    {
        private static MTGPhase instance;
        
        MTGPhase GetInstance() { return instance ?? new MTGCleanupStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private new string step = "Cleanup";
    }
}