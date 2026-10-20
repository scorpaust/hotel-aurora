#nullable enable

using UnityEngine;

namespace Aurora.Core
{
    /// <summary>
    /// Placeholder until F2-03 (Memórias e livro de hóspedes) implements the full content.
    /// Exists now so BossDefinition (Aurora.Hotel) has a stable type to reference.
    /// </summary>
    [CreateAssetMenu(menuName = "Aurora/Content/Guest Book Entry (placeholder)", fileName = "SO_NewGuestBookEntry")]
    public sealed class GuestBookEntry : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;

        public string Id => _id;
    }
}
