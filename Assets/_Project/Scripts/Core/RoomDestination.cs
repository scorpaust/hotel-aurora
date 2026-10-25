#nullable enable

using UnityEngine;

namespace Aurora.Core
{
    /// <summary>
    /// Where a RoomConnection leads. Empty SceneName means the target Room is already
    /// loaded (same ala); a non-empty SceneName tells the room-transition service to
    /// load that Addressable scene first (see F1-02 spec item 3). Lives in Core so both
    /// Aurora.Gameplay (RoomConnection) and Aurora.Hotel (the resolver) can share it.
    /// </summary>
    [System.Serializable]
    public struct RoomDestination
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private string _roomId;
        [SerializeField] private string _spawnId;

        public RoomDestination(string roomId, string spawnId, string sceneName = "")
        {
            _roomId = roomId;
            _spawnId = spawnId;
            _sceneName = sceneName;
        }

        public string SceneName => _sceneName;
        public string RoomId => _roomId;
        public string SpawnId => _spawnId;
        public bool IsCrossScene => !string.IsNullOrEmpty(_sceneName);
        public bool IsValid => !string.IsNullOrEmpty(_roomId) && !string.IsNullOrEmpty(_spawnId);
    }
}
