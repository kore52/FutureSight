﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public enum TriggerType
    {
        Null,
        AtUntap,                // player
        AtUpkeep,               // player
        AtDraw,                 // player
        AtEndOfTurn,            // player
        AtBeginOfCombat,        // player
        AtEndOfCombat,          // player
        WhenDamageIsDealt,      // damage
        WhenOtherSpellIsCast,   // card on stack
        WhenSpellIsCast,        // card on stack
        WhenComesIntoPlay,      // controller
        WhenLeavesPlay,         // MagicRemoveFromPlayAction
        WhenBecomesTapped,      // permanent
        WhenBecomesUntapped,    // permanent
        WhenDrawn,              // card
        WhenOtherDrawn,         // card
        WhenLifeIsGained,       // player, life gained
        WhenLifeIsLost,         // player, life lost
        WhenOtherComesIntoPlay, // permanent
        WouldBeMoved,           // MagicMoveCardAction
        WhenPutIntoGraveyard,       // graveyard trigger data
        WhenOtherPutIntoGraveyard,  // graveyard trigger data
        WhenOtherDies,          // permanent
        WhenAttacks,            // permanent
        WhenBlocks,             // permanent
        WhenBecomesBlocked,     // permanent
        WhenAttacksUnblocked,   // permanent
        WhenTargeted,           // permanent
        WhenLoseControl,        // permanent
        WhenBecomesState,       // MagicChangeStateAction
        WhenTurnedFaceUp,       // permanent
        WhenChampioned,         // MagicExiledUntilThisLeavesPlayAction
        WhenCycle,              // card
        WhenOtherCycle,         // card
        WhenScry,               // player
        IfDamageWouldBeDealt,   // item on stack
        IfPlayerWouldLose,      // player[]
        IfLifeWouldChange,      // MagicChangeLifeAction
        WhenClash,              // MagicPlayer
        Protection,             // MagicPermanent
        CannotBeBlocked,        // MagicPermanent
        CantBlock,              // MagicPermanent
    }

    public class MTGTrigger
    {
        public TriggerType Type { get { return TriggerType.Null; } }
    }
}
