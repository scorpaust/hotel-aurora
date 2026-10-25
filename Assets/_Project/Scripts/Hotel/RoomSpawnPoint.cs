#nullable enable

using UnityEngine;

namespace Aurora.Hotel
{
    public sealed class RoomSpawnPoint : MonoBehaviour
    {
        [SerializeField] private string _id = string.Empty;

        public string Id => _id;

        /// <summary>Editor-authoring helper.</summary>
        public void SetId(string id) => _id = id;
    }
}
