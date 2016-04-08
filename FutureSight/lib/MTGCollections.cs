using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FutureSight.lib
{
    public class MTGEventQueue
    {
        private LinkedList<MTGEvent> Events;
        
        public MTGEventQueue()
        {
            Events = new LinkedList<MTGEvent>();
        }
        
        public MTGEventQueue(LinkedList<MTGEvent> queue)
        {
            Events = new LinkedList<MTGEvent>(queue);
        }
        
        public MTGEvent First { get { return Events.First.Value; } }
        public MTGEvent Last { get { return Events.Last.Value; } }
        
        public void AddFirst(MTGEvent item)
            => Events.AddFirst(item);
        
        public void AddLast(MTGEvent item)
            => Events.AddLast(item);
        
        public MTGEvent RemoveFirst()
        {
            var e = Events.First.Value;
            Events.RemoveFirst();
            return e;
        }
        
        public MTGEvent RemoveLast()
        {
            var e = Events.Last.Value;
            Events.RemoveLast();
            return e;
        }
        
        public void Clear()
            => Events.Clear();
        
        public int Count { get { return Events.Count; } }
        
        public bool IsEmpty()
            => Events.Count == 0;
    }
}