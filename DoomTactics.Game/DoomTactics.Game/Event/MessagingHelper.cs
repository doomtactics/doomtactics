using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class MessagingHelper
    {
        private struct CallbackWithType
        {
            public Action<IDoomEvent> callback;
            public DoomEventType type;
            public string ListenerName;
        }

        private List<CallbackWithType> callbacks;

        public MessagingHelper()
        {
            callbacks = new List<CallbackWithType>();
        }
      
        public void Subscribe(Action<IDoomEvent> callback, DoomEventType type, string name)
        {
            callbacks.Add(new CallbackWithType() { callback = callback, type = type, ListenerName = name });
        }

        public void DispatchEvent(IDoomEvent evt)
        {
            var interestedListeners =
                from l in callbacks
                where l.type == evt.EventType
                      && (evt.ListenerNames == null || evt.ListenerNames.Length == 0 || evt.ListenerNames.Contains(l.ListenerName))
                select l;
            foreach (var intListener in interestedListeners)
            {
                intListener.callback(evt);
            }
        }

    }
}
