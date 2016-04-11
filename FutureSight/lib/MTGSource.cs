using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FutureSight.lib
{
    public class MTGSource : MTGObject
    {
        public MTGPlayer Controller { get; set; }
        
        public MTGSource() {}
        public MTGSource(MTGPlayer controller)
        {
            Controller = controller;
        }
        
        public bool IsController(MTGPlayer player)
          => Controller.Equals(player);
        
    }
}