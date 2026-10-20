#nullable enable

using Aurora.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aurora.Editor
{
    internal static class PlayFromAnywhereBootstrapper
    {
        private const string BootSceneName = "Boot";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureBootLoaded()
        {
            BootstrapFlags.PlayFromAnywhere = false;

            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == BootSceneName)
                return;

            BootstrapFlags.PlayFromAnywhere = true;
            SceneManager.LoadScene(BootSceneName, LoadSceneMode.Additive);
        }
    }
}
