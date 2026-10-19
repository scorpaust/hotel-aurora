#nullable enable

using System;
using System.Threading;
using UnityEngine;

namespace Aurora.Core
{
    public interface ISceneService
    {
        event Action<string, float> LoadProgressChanged;

        Awaitable LoadAdditiveAsync(string sceneName, CancellationToken cancellationToken = default);
        Awaitable UnloadAsync(string sceneName, CancellationToken cancellationToken = default);
    }
}
