#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Aurora.Core
{
    public sealed class SceneService : ISceneService
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes = new();

        public event Action<string, float>? LoadProgressChanged;

        public async Awaitable LoadAdditiveAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            var handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!handle.IsDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                LoadProgressChanged?.Invoke(sceneName, handle.PercentComplete);
                await Awaitable.NextFrameAsync(cancellationToken);
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"[SceneService] Failed to load scene '{sceneName}'.");
                return;
            }

            _loadedScenes[sceneName] = handle;
            LoadProgressChanged?.Invoke(sceneName, 1f);
        }

        public async Awaitable UnloadAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            if (!_loadedScenes.Remove(sceneName, out var handle))
            {
                Debug.LogError($"[SceneService] No loaded scene registered for '{sceneName}'.");
                return;
            }

            var unloadHandle = Addressables.UnloadSceneAsync(handle);
            while (!unloadHandle.IsDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Awaitable.NextFrameAsync(cancellationToken);
            }
        }
    }
}
