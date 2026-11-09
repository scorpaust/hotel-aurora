#nullable enable

using System.Collections.Generic;

namespace Aurora.Core
{
    /// <summary>
    /// Picks the best IInteractable candidate: highest Priority, then closest distance.
    /// Pure and testable — PlayerInteractor is responsible for filtering by CanInteract()
    /// before calling this.
    /// </summary>
    public static class InteractableSelector
    {
        public static IInteractable? SelectBest(IReadOnlyList<(IInteractable interactable, float distance)> candidates)
        {
            IInteractable? best = null;
            var bestPriority = int.MinValue;
            var bestDistance = float.MaxValue;

            foreach (var (interactable, distance) in candidates)
            {
                var isBetter = interactable.Priority > bestPriority ||
                               (interactable.Priority == bestPriority && distance < bestDistance);

                if (!isBetter)
                    continue;

                best = interactable;
                bestPriority = interactable.Priority;
                bestDistance = distance;
            }

            return best;
        }
    }
}
