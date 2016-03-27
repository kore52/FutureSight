using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class MTGChoice
    {
        public MTGChoice() { }
    }

    public class MTGChoiceResults
    {
        public List<object> Results { get; set; }
    }

    public class MTGCardChoice : MTGChoice
    {
        public List<MTGCard> GetResults(GameState state, MTGEvent aEvent)
        {
            return new List<MTGCard>();
        }
    }
}
