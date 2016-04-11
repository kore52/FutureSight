using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    public class ResolveStackAction : MTGAction
    {
        private MTGStackItem itemOnStack;
        
        public ResolveStackAction() {}

        public override void DoAction(MTGGame game)
        {
            itemOnStack = game.Stack.Last.Value;
            game.Stack.RemoveLast();
            itemOnStack.Resolve(game);
        }

        public override void UndoAction(MTGGame game)
        {
            game.Stack.AddLast(itemOnStack);
        }
    }
}