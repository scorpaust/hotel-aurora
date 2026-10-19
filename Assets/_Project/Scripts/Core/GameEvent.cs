#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aurora.Core
{
    [CreateAssetMenu(menuName = "Aurora/Events/Game Event", fileName = "SO_NewGameEvent")]
    public sealed class GameEvent : ScriptableObject
    {
        private readonly List<Action> _listeners = new();

        public void Raise()
        {
            var snapshot = _listeners.ToArray();
            foreach (var listener in snapshot)
                listener?.Invoke();
        }

        public void Register(Action listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void Unregister(Action listener)
        {
            _listeners.Remove(listener);
        }
    }
}
