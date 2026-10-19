#nullable enable

using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aurora.Core
{
    public sealed class SceneService : ISceneService
    {
        public event Action<string, float>? LoadProgressChanged;

        public async Awaitable LoadAdditiveAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (operation == null)
            {
                Debug.LogError($"[SceneService] Failed to start loading scene '{sceneName}'.");
                return;
            }

            while (!operation.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                LoadProgressChanged?.Invoke(sceneName, operation.progress);
                await Awaitable.NextFrameAsync(cancellationToken);
            }

            LoadProgressChanged?.Invoke(sceneName, 1f);
        }

        public async Awaitable UnloadAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            if (operation == null)
            {
                Debug.LogError($"[SceneService] Failed to start unloading scene '{sceneName}'.");
                return;
            }

            while (!operation.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Awaitable.NextFrameAsync(cancellationToken);
            }
        }
    }
}
