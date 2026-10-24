#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Platform
{
    /// <summary>
    /// Registers <see cref="IPlatformServices"/>. Runs in Start() rather than Awake() so it
    /// always executes after GameBootstrap's Awake() — which calls Services.Reset() — regardless
    /// of script execution order; Aurora.Input never references Aurora.Platform, so this can't be
    /// registered from GameBootstrap directly.
    /// </summary>
    public sealed class PlatformBootstrap : MonoBehaviour
    {
        private void Start()
        {
            IPlatformServices platform = new NullPlatform();

#if UNITY_WEBGL && !STEAM_BUILD
            platform = new WebPlatform();
#endif
            // Under STEAM_BUILD, NullPlatform registers first as a safe fallback; Aurora.Platform.Steam
            // (which references this assembly, not the other way around) re-registers its own
            // SteamPlatform once F7-02 implements it, overwriting this entry.
            Services.Register(platform);
        }
    }
}
