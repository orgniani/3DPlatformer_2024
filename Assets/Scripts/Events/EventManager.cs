using System.Collections.Generic;
using UnityEngine;
using Core.Singletons;

namespace Events
{
    //TODO: Review script
    public delegate void EventDelegate(params object[] args);

    [DisallowMultipleComponent]
    public abstract class EventManager<IdT> : MonoBehaviourSingleton<EventManager<IdT>>
    {
        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private Dictionary<IdT, EventDelegate> _events = new();
        public void InvokeEvent(IdT eventIdentifier, params object[] args)
        {
            if (_events.TryGetValue(eventIdentifier, out var eventDelegate))
            {
                eventDelegate?.Invoke(args);
                if (enableLogs) Debug.Log($"{name}: Event <color=cyan>({eventIdentifier})</color> invoked");
            }
            else
                if (enableLogs) Debug.LogWarning($"{name}: Event id <color=yellow>({eventIdentifier})</color> had no subscribers!");
        }

        public void SubscribeToEvent(IdT eventIdentifier, EventDelegate handler)
        {
            if (!_events.TryAdd(eventIdentifier, handler))
                _events[eventIdentifier] += handler;
            else
                if (enableLogs) Debug.Log($"{name}: Event id <color=green>({eventIdentifier})</color> was added");
        }

        public void UnsubscribeFromEvent(IdT eventIdentifier, EventDelegate handler)
        {
            if (_events.ContainsKey(eventIdentifier))
            {
                _events[eventIdentifier] -= handler;
                if (_events[eventIdentifier] == null)
                {
                    _events.Remove(eventIdentifier);
                    if (enableLogs) Debug.Log($"{name}: Event id <color=black>({eventIdentifier})</color> was removed");
                }
            }
        }
    }
}
