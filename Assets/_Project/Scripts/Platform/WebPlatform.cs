#nullable enable

using Aurora.Core;

namespace Aurora.Platform
{
    /// <summary>
    /// Platform services for Web builds. No browser achievement/cloud API to call yet — behaves
    /// like NullPlatform for now, kept as its own type so Web-specific hooks (analytics, share
    /// links) have somewhere to go without touching the Null/Steam paths.
    /// </summary>
    public sealed class WebPlatform : IPlatformServices
    {
        public string PlatformName => "Web";
        public bool SupportsAchievements => false;
        public bool SupportsCloudSave => false;

        public void UnlockAchievement(string id) { }

        public void SetRichPresence(string status) { }
    }
}
