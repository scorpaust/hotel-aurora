#nullable enable

namespace Aurora.Core
{
    /// <summary>
    /// In-memory world flags (`ala.objeto.estado` -> int), see codespec.md §5.5.
    /// F1-06 wraps this same interface with save/load persistence.
    /// </summary>
    public interface IWorldFlagsService
    {
        int GetFlag(string key, int defaultValue = 0);
        void SetFlag(string key, int value);
        bool HasFlag(string key);
    }
}
