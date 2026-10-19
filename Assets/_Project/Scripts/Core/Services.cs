#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aurora.Core
{
    public static class Services
    {
        private static readonly Dictionary<Type, object> Registry = new();

        public static void Register<T>(T instance) where T : class
        {
            Registry[typeof(T)] = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public static T Get<T>() where T : class
        {
            if (Registry.TryGetValue(typeof(T), out var instance))
                return (T)instance;

            throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
        }

        public static bool TryGet<T>(out T instance) where T : class
        {
            if (Registry.TryGetValue(typeof(T), out var value))
            {
                instance = (T)value;
                return true;
            }

            instance = null!;
            return false;
        }

        public static void Reset()
        {
            Registry.Clear();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnDomainReload()
        {
            Reset();
        }
    }
}
