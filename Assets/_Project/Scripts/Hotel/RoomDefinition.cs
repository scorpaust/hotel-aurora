#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Hotel
{
    /// <summary>
    /// Minimal placeholder until F1-02 (salas, câmara e transições) develops room
    /// connections/spawn points for real. For now it only carries what F1-10 needs:
    /// whether the room has a lantern past-layer and which LevelFlowDefinition it uses.
    /// </summary>
    [CreateAssetMenu(menuName = "Aurora/Content/Room Definition", fileName = "SO_NewRoom")]
    public sealed class RoomDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private string _displayName = string.Empty;

        [Tooltip("Whether this room has a lantern past-layer to reveal (F1-04).")]
        [SerializeField] private bool _hasPastLayer;

        [SerializeField] private LevelFlowDefinition? _flow;

        public string Id => _id;
        public string DisplayName => _displayName;
        public bool HasPastLayer => _hasPastLayer;
        public LevelFlowDefinition? Flow => _flow;
    }
}
