using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    class GameTree
    {
        public GameTree()
        {
            Node = new List<GameState>();
        }
        public List<GameState> Node { get; set; }
    }
}
