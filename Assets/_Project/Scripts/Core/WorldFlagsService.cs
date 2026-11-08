#nullable enable

using System.Collections.Generic;

namespace Aurora.Core
{
    public sealed class WorldFlagsService : IWorldFlagsService
    {
        private readonly Dictionary<string, int> _flags = new();

        public int GetFlag(string key, int defaultValue = 0) =>
            _flags.TryGetValue(key, out var value) ? value : defaultValue;

        public void SetFlag(string key, int value) => _flags[key] = value;

        public bool HasFlag(string key) => _flags.ContainsKey(key);
    }
}
