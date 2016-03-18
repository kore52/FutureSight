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
        /*
        Element for event
        
        spell or ability
         mode
          choice : 1, 2
           target
            targettype = {
             none, player, object, zone,
             1, 2, 1 or more, 3 or less, etc...
            }
            choice
           cost
            costtype: mana, tap, loyalty, scrifice, paylife, alternative, additional, variable, etc...
             choice
            choice
          effect
           list of event
            event
             eventtype: etb, pig, exile, put onto top of library, etc...
                        look, draw, discard, tap, untap, add mana, pay mana, deal damage, prevent damage,
                        put any counters onto
                        divide any pile
                        copy
                        cast
             choice
        */
    interface abstract class MTGEvent
    {
    }
    
    interface abstract class MTGCost
    {
    }
    
    public class MTGDrawEvent : MTGEvent
    {
    }
    
    
    public class MTGChoice
    {
        public MTGChoice() {}
    }
    
    public class MTGCardChoice : MTGChoice
    {
        public List<Card> GetResults(GameState state, MTGEvent event)
    }
}