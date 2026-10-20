#nullable enable

namespace Aurora.Core
{
    /// <summary>
    /// Common contract for data-driven content ScriptableObjects (enemies, bosses, traps,
    /// cutscenes, audio cues, narrative beats, level flows...), so Editor tooling can validate
    /// and reference them generically without depending on each concrete assembly.
    /// </summary>
    public interface IContentDefinition
    {
        string Id { get; }
    }
}
