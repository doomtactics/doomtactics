using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public static class MessagingSystem
    {
        private static MessagingHelper _helper = new MessagingHelper();
        private static Queue<IDoomEvent> _queue = new Queue<IDoomEvent>();

        public static void Subscribe(Action<IDoomEvent> callback, DoomEventType eventType, string name)
        {
            _helper.Subscribe(callback, eventType, name);
        }

        public static void DispatchEvent(IDoomEvent evt)
        {
            _queue.Enqueue(evt);
        }

        public static void ProcessQueued()
        {
            while (_queue.Count > 0)
            {
                var evt = _queue.Dequeue();
                _helper.DispatchEvent(evt);
            }
        }
    }
}
