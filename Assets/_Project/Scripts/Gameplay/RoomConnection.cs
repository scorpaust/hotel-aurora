#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Gameplay
{
    /// <summary>
    /// A door/stairwell/elevator/passage trigger zone. PlayerController detects overlap
    /// (same pattern as StairZone/Pushable) and calls Activate() on Interact; this builds
    /// a RoomTransitionRequest and hands it to IRoomTransitionService (Aurora.Hotel) —
    /// RoomConnection itself never resolves rooms, so Gameplay never needs to reference
    /// Hotel (see F1-02 spec items 2-3).
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class RoomConnection : MonoBehaviour
    {
        [SerializeField] private RoomConnectionType _type = RoomConnectionType.Door;
        [SerializeField] private RoomDestination _destination;

        [Header("Optional condition (WorldFlags)")]
        [SerializeField] private string _conditionFlagKey = string.Empty;
        [SerializeField] private int _conditionRequiredValue;
        [SerializeField] private RoomDestination _fallbackDestination;

        private void Reset()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        /// <summary>Editor-authoring helper — lets tooling set these private fields without SerializedObject.</summary>
        public void Configure(
            RoomConnectionType type,
            RoomDestination destination,
            string conditionFlagKey = "",
            int conditionRequiredValue = 0,
            RoomDestination fallbackDestination = default)
        {
            _type = type;
            _destination = destination;
            _conditionFlagKey = conditionFlagKey;
            _conditionRequiredValue = conditionRequiredValue;
            _fallbackDestination = fallbackDestination;
        }

        public void Activate()
        {
            if (!Services.TryGet<IRoomTransitionService>(out var transitions) || transitions.IsTransitioning)
                return;

            var request = new RoomTransitionRequest(
                _type, _destination, _conditionFlagKey, _conditionRequiredValue, _fallbackDestination);

            _ = transitions.RequestTransitionAsync(request);
        }
    }
}
