#nullable enable

namespace Aurora.Core
{
    /// <summary>Pure destination resolution for a RoomTransitionRequest (F1-02 spec item 2), kept out of any MonoBehaviour so it's directly testable.</summary>
    public static class RoomTransitionResolver
    {
        public static RoomDestination Resolve(RoomTransitionRequest request, int currentFlagValue)
        {
            if (!request.HasCondition)
                return request.Destination;

            return currentFlagValue == request.ConditionRequiredValue
                ? request.Destination
                : request.FallbackDestination;
        }
    }
}
