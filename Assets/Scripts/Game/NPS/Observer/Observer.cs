using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPS
{
    public class Observer: MonoSingleton<Observer>
    {
        private Dictionary<EventID, Action<object>> listeners = new Dictionary<EventID, Action<object>>();

        public override void OnDestroy()
        {
            base.OnDestroy();

            ClearAllListener();
        }

        private void ClearAllListener()
        {
            listeners.Clear();
        }

        public void RegisterListener(EventID eventID, Action<object> callback)
        {
            if (!listeners.ContainsKey(eventID))
                listeners.Add(eventID, null);

            listeners[eventID] += callback;
        }

        public void RemoveListener(EventID eventID, Action<object> callback)
        {
            if (!listeners.ContainsKey(eventID)) return;
            if (callback == null) return;

            listeners[eventID] -= callback;
        }

        public void PostEvent(EventID eventID, object param = null)
        {
            if (!listeners.ContainsKey(eventID)) return;

            var callbacks = listeners[eventID];
            if (callbacks != null) callbacks(param);
            else listeners.Remove(eventID);
        }        
    }

    public static class ObserverExtension
    {
        public static void RegisterListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
        {
            Observer.S?.RegisterListener(eventID, callback);
        }

        public static void RemoveListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
        {
            Observer.S?.RemoveListener(eventID, callback);
        }

        public static void PostEvent(this MonoBehaviour sender, EventID eventID, object param = null)
        {
            Observer.S?.PostEvent(eventID, param);
        }
    }
}