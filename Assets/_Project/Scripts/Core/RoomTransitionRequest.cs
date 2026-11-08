#nullable enable

namespace Aurora.Core
{
    /// <summary>
    /// What a RoomConnection (Aurora.Gameplay) asks the room-transition service
    /// (Aurora.Hotel) to do — see F1-02 spec items 2-3.
    /// </summary>
    public readonly struct RoomTransitionRequest
    {
        public RoomConnectionType Type { get; }
        public RoomDestination Destination { get; }
        public string ConditionFlagKey { get; }
        public int ConditionRequiredValue { get; }
        public RoomDestination FallbackDestination { get; }

        public bool HasCondition => !string.IsNullOrEmpty(ConditionFlagKey);

        public RoomTransitionRequest(
            RoomConnectionType type,
            RoomDestination destination,
            string conditionFlagKey,
            int conditionRequiredValue,
            RoomDestination fallbackDestination)
        {
            Type = type;
            Destination = destination;
            ConditionFlagKey = conditionFlagKey;
            ConditionRequiredValue = conditionRequiredValue;
            FallbackDestination = fallbackDestination;
        }
    }
}
