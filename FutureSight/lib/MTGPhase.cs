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
                    // �X�e�b�v�ŗL�����̎��s
                    ExecuteBeginPhase(game);
                    game.Update();
                    break;
                case MTGStep.ActivePlayer:
                    // �󋵋N�������̎��s
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

        public virtual MTGPhaseType Type { get { return MTGPhaseType.Null; } }
    }
    
    // �X�e�b�v�ŗL����
    
    /// <summary>
    /// �}���K���X�e�b�v
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.Mulligan; } }
    }

    /// <summary>
    /// �A���^�b�v�X�e�b�v
    /// </summary>
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
            game.Log(player, "untap");
            foreach (var permanent in player.Permanents)
            {
                // ���Ґ����̉���
                if (permanent.HasState(MTGPermanentState.Summoned))
                    game.DoAction(ChangeStateAction.Clear(permanent, MTGPermanentState.Summoned));
                
                // �A���^�b�v
                if (permanent.IsTapped())
                    game.DoAction(new UntapAction(permanent));
            }
        }

        public override MTGPhaseType Type { get { return MTGPhaseType.Untap; } }
    }
    
    /// <summary>
    /// �A�b�v�L�[�v�X�e�b�v
    /// </summary>
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
            game.Step = game.CanSkipPhase() ? MTGStep.NextPhase: MTGStep.ActivePlayer;
        }
        public override void ExecuteEndOfPhase(GameState game)
        {
        }

        public override MTGPhaseType Type { get { return MTGPhaseType.Upkeep; } }
    }
    
    /// <summary>
    /// �h���[�X�e�b�v
    /// </summary>
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
            // 103.7a 2�l�ΐ�ł́A��U�̃v���C���[�̍ŏ��̃^�[���̃h���[�E�X�e�b�v���΂�
            if (game.Turn == 1)
            {
                game.Step = MTGStep.NextPhase;
                return;
            }
            
            // �h���[�A�N�V�����𐶐�
            game.DoAction(new DrawAction(game.TurnPlayer, 1));
            
            game.Step = game.CanSkipPhase() ? MTGStep.NextPhase: MTGStep.ActivePlayer;
        }
        public override void ExecuteEndOfPhase(GameState game)
        {
        }

        public override MTGPhaseType Type { get { return MTGPhaseType.Draw; } }
    }
    
    /// <summary>
    /// ��ꃁ�C���t�F�C�Y
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.FirstMain; } }
    }
    
    /// <summary>
    /// �퓬�J�n�X�e�b�v
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.PreCombat; } }
    }
    
    /// <summary>
    /// �U���N���[�`���[�w��X�e�b�v
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.DeclareAttacker; } }
    }
    
    /// <summary>
    /// �u���b�N�N���[�`���[�w��X�e�b�v
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.DeclareBlocker; } }
    }
    
    /// <summary>
    /// �퓬�_���[�W�X�e�b�v
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.Damage; } }

    }
    
    /// <summary>
    /// �퓬�I���X�e�b�v
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.EndCombat; } }
    }
    
    /// <summary>
    /// ��񃁃C���t�F�C�Y
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.SecondMain; } }
    }
    
    /// <summary>
    /// �I���X�e�b�v
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.End; } }
    }
    
    /// <summary>
    /// �N�����i�b�v�X�e�b�v
    /// </summary>
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

        public override MTGPhaseType Type { get { return MTGPhaseType.Cleanup; } }
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
            Console.WriteLine(game.Phase.GetType());
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
                default:
                    if (game.Phase == null ) throw new Exception("Phase is NULL");
                    throw new Exception("Invalid Phase Type: " + game.Phase.GetType() + ":" + game.Phase.Type);
            }
            return null;
        }
    }
}