#nullable enable

using UnityEngine;

namespace Aurora.Core
{
    /// <summary>
    /// Placeholder until F2-03 (Memórias e livro de hóspedes) implements the full content
    /// (fragmentos, cutscene associada, efeito na história). Exists now so BossDefinition
    /// (Aurora.Hotel) has a stable type to reference.
    /// </summary>
    [CreateAssetMenu(menuName = "Aurora/Content/Memory Definition (placeholder)", fileName = "SO_NewMemory")]
    public sealed class MemoryDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private string _wing = string.Empty;

        public string Id => _id;
        public string Wing => _wing;
    }
}
