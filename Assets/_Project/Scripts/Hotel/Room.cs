#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Aurora.Hotel
{
    /// <summary>
    /// A room in-scene: wraps a RoomDefinition, exposes its camera-confiner bounds
    /// and named spawn points, and registers itself by id so RoomConnection/
    /// RoomTransitionController can resolve destinations (see F1-02 spec item 1).
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class Room : MonoBehaviour
    {
        private static readonly Dictionary<string, Room> RegistryById = new();

        [SerializeField] private RoomDefinition? _definition;
        [SerializeField] private Collider2D? _bounds;

        public string Id => _definition != null ? _definition.Id : string.Empty;
        public RoomDefinition? Definition => _definition;
        public Collider2D? Bounds => _bounds;

        /// <summary>
        /// Editor-authoring helper — lets tooling set these private fields without
        /// SerializedObject. Re-registers under the new Id: callers that invoke this
        /// right after AddComponent&lt;Room&gt;() (where OnEnable already ran with no
        /// definition set) still end up correctly registered.
        /// </summary>
        public void Configure(RoomDefinition definition, Collider2D bounds)
        {
            Unregister();
            _definition = definition;
            _bounds = bounds;

            if (isActiveAndEnabled)
                Register();
        }

        public static bool TryGetById(string id, out Room room)
        {
            if (RegistryById.TryGetValue(id, out var found))
            {
                room = found;
                return true;
            }

            room = null!;
            return false;
        }

        private void OnEnable() => Register();

        private void OnDisable() => Unregister();

        private void Register()
        {
            if (!string.IsNullOrEmpty(Id))
                RegistryById[Id] = this;
        }

        private void Unregister()
        {
            if (!string.IsNullOrEmpty(Id) && RegistryById.TryGetValue(Id, out var room) && room == this)
                RegistryById.Remove(Id);
        }

        /// <summary>
        /// Scans children live rather than caching in Awake — Configure() and a
        /// RoomSpawnPoint child can be added in either order by authoring code, and
        /// this is only ever called at transition time, never per-frame.
        /// </summary>
        public bool TryGetSpawnPoint(string spawnId, out RoomSpawnPoint spawnPoint)
        {
            foreach (var candidate in GetComponentsInChildren<RoomSpawnPoint>(includeInactive: true))
            {
                if (candidate.Id == spawnId)
                {
                    spawnPoint = candidate;
                    return true;
                }
            }

            spawnPoint = null!;
            return false;
        }
    }
}
