using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

using FutureSight.lib.action;

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
                    ExecuteEndOfPhase(game);
                    game.Update();
                    game.NextPhase();
                    break;
            }
        }
        public abstract void ExecuteBeginPhase(GameState game);
        public abstract void ExecuteEndOfPhase(GameState game);

        public MTGPhaseType Type { get { return MTGPhaseType.Null; } }
    }
    
    // ステップ固有処理
    public class MTGMulligunPhase : MTGPhase
    {
        private static MTGPhase instance;

        public static MTGPhase GetInstance()
        {
            if (instance == null) { instance = new MTGMulligunPhase(); }
            return instance;
        }

        public override void ExecuteBeginPhase(GameState game)
        {
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
        {

        }

        public new MTGPhaseType Type { get { return MTGPhaseType.Mulligan; } }
    }

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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
        {

        }

        private void Untap(GameState game)
        {
            var player = game.GetActivePlayer();
            foreach (var permanent in player.Permanents)
            {
                if (permanent.HasState(MTGPermanentState.Summoned))
                    game.DoAction(new ChangeStateAction(permanent, MTGPermanentState.Summoned, false));

                if (permanent.IsTapped())
                {
                    game.DoAction(new UntapAction(permanent));
                }
            }
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
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
            game.Step = MTGStep.NextPhase;
        }
        public override void ExecuteEndOfPhase(GameState game)
        {
        }

        public new MTGPhaseType Type { get { return MTGPhaseType.Cleanup; } }
    }

    public abstract class MTGGamePlay
    {
        public abstract MTGPhase GetStartPhase(GameState game);
        public abstract MTGPhase NextPhase(GameState game);
    }

    public class MTGDefaultGamePlay : MTGGamePlay
    {
        private static MTGDefaultGamePlay instance;
        public static MTGDefaultGamePlay GetInstance()
        {
            if (instance == null) instance = new MTGDefaultGamePlay();
            return instance;
        }

        public override MTGPhase GetStartPhase(GameState game)
        {
            return MTGMulligunPhase.GetInstance();
        }

        public override MTGPhase NextPhase(GameState game)
        {
            switch (game.Phase.Type)
            {
                case MTGPhaseType.Mulligan:
                    return MTGUntapStep.GetInstance();
                case MTGPhaseType.Untap:
                    return MTGUpkeepStep.GetInstance();
                case MTGPhaseType.Upkeep:
                    return MTGDrawStep.GetInstance();
                case MTGPhaseType.Draw:
                    return MTGFirstMainPhase.GetInstance();
                case MTGPhaseType.FirstMain:
                    return MTGPreCombatStep.GetInstance();
                case MTGPhaseType.PreCombat:
                    return MTGDeclareAttackerStep.GetInstance();
                case MTGPhaseType.DeclareAttacker:
                    return MTGDeclareBlockerStep.GetInstance();
                case MTGPhaseType.DeclareBlocker:
                    return MTGDamageStep.GetInstance();
                case MTGPhaseType.Damage:
                    return MTGEndCombatStep.GetInstance();
                case MTGPhaseType.EndCombat:
                    return MTGSecondMainPhase.GetInstance();
                case MTGPhaseType.SecondMain:
                    return MTGEndStep.GetInstance();
                case MTGPhaseType.End:
                    return MTGCleanupStep.GetInstance();
                case MTGPhaseType.Cleanup:
                    return MTGUntapStep.GetInstance();
            }
            return null;
        }
    }
}