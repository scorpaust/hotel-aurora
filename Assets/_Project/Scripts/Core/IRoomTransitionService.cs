#nullable enable

using System.Threading;
using UnityEngine;

namespace Aurora.Core
{
    public interface IRoomTransitionService
    {
        bool IsTransitioning { get; }

        Awaitable RequestTransitionAsync(RoomTransitionRequest request, CancellationToken cancellationToken = default);
    }
}
