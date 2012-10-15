using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace DoomTactics
{
    public class MessagingHelper
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        private struct CallbackWithType
        {
            public Action<IDoomEvent> callback;
            public DoomEventType type;
            public string ListenerName;
            public string SenderName;
        }

        private List<CallbackWithType> callbacks;

        public MessagingHelper()
        {
            callbacks = new List<CallbackWithType>();
        }
      
        public void Subscribe(Action<IDoomEvent> callback, DoomEventType type, string subscriberName, string senderName)
        {
            callbacks.Add(new CallbackWithType() { callback = callback, type = type, ListenerName = subscriberName, SenderName = senderName });
        }

        public void Unsubscribe(string name)
        {
            callbacks.RemoveAll(x => x.ListenerName == name);
        }

        public void DispatchEvent(IDoomEvent evt, string senderName)
        {
            var interestedListeners =
                from l in callbacks
                where l.type == evt.EventType
                      && (l.SenderName == null || l.SenderName == senderName)
                      && (evt.ListenerNames == null || evt.ListenerNames.Length == 0 || evt.ListenerNames.Contains(l.ListenerName))
                select l;
            foreach (var intListener in interestedListeners)
            {
                //_log.Debug("Found interested listener " + intListener.ListenerName + " for event " + evt);
                intListener.callback(evt);
            }
        }

    }
}
