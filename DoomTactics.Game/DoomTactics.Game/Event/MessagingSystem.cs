using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace DoomTactics
{
    public struct EventWithSender
    {
        public IDoomEvent DoomEvent;
        public string SenderName;
        
        public EventWithSender(IDoomEvent evt, string senderName)
        {
            DoomEvent = evt;
            SenderName = senderName;
        }

        public override string ToString()
        {
            return "Event: " + Enum.GetName(typeof (DoomEventType), DoomEvent.EventType) + " sent by " + SenderName;
        }
    }

    public static class MessagingSystem
    {
        private static MessagingHelper _helper = new MessagingHelper();
        private static Queue<EventWithSender> _queue = new Queue<EventWithSender>();
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public static void Subscribe(Action<IDoomEvent> callback, DoomEventType eventType, string subscriberName, string senderName)
        {
            _log.Debug("Subscription from " + subscriberName + " to events " + Enum.GetName(typeof(DoomEventType), eventType) + " by " + senderName);
            _helper.Subscribe(callback, eventType, subscriberName, senderName);
        }

        public static void Unsubscribe(string name)
        {
            _helper.Unsubscribe(name);
        }

        public static void DispatchEvent(IDoomEvent evt, string senderName)
        {
            _queue.Enqueue(new EventWithSender(evt, senderName));
        }

        public static void ProcessQueued()
        {
            while (_queue.Count > 0)
            {                
                var evt = _queue.Dequeue();
                _log.Debug(evt);
                _helper.DispatchEvent(evt.DoomEvent, evt.SenderName);
            }
        }
    }
}
