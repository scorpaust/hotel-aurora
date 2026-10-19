#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aurora.Core
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        private readonly List<Action<T>> _listeners = new();

        public void Raise(T value)
        {
            var snapshot = _listeners.ToArray();
            foreach (var listener in snapshot)
                listener?.Invoke(value);
        }

        public void Register(Action<T> listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void Unregister(Action<T> listener)
        {
            _listeners.Remove(listener);
        }
    }
}
