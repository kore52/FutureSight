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
        public string ToString() { return step; }
        protected virtual void Execute(GameState game) { }
        protected virtual string step = "";
    }
    
    // ステップ固有処理
    public class MTGUntapStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGUntapStep(); }
        
        public void ExecutePhase(GameState game)
        {
        }
        
        private string step = "Untap";
    }
    
    public class MTGUpkeepStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGUpkeepStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "Upkeep";
    }
    
    public class MTGDrawStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGDrawStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "Draw";
    }

    public class MTGFirstMainPhase : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGFirstMainPhase(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "FirstMain";
    }

    public class MTGPreCombatStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGPreCombatStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "PreCombat";
    }

    public class MTGDeclareAttackerStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGDeclareAttackerStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "DeclareAttacker";
    }

    public class MTGDeclareBlockerStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGDeclareBlockerStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "DeclareBlocker";
    }

    public class MTGDamageStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGDamageStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "Damage";
    }

    public class MTGEndCombatStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGEndCombatStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "EndCombat";
    }

    public class MTGSecondMainPhase : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGSecondMainPhase(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "SecondMain";
    }

    public class MTGEndStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGEndStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "End";
    }

    public class MTGCleanupStep : public MTGPhase
    {
        private static MTGPhase instance;
        
        public MTGPhase GetInstance() { return instance ?? new MTGCleanupStep(); }
        
        public void ExecuteStep(GameState game)
        {
        }
        
        private string step = "Cleanup";
    }
}