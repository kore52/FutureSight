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
    public class MTGEvent
    {
        delegate void MTGEventHandler(GameState state, PlayerState player);
    }
    
    public class MTGDrawEvent : MTGEvent
    {
        public void Execute(GameState state, PlayerState player)
        {
            player.Draw();
        }
    }
    
    public class MTGChoice
    {
        public MTGChoice {}
    }
}