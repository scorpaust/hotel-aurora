#nullable enable

using Aurora.Core;

namespace Aurora.Platform
{
    /// <summary>No-op platform services: Editor, and Windows builds without Steam.</summary>
    public sealed class NullPlatform : IPlatformServices
    {
        public string PlatformName => "None";
        public bool SupportsAchievements => false;
        public bool SupportsCloudSave => false;

        public void UnlockAchievement(string id) { }

        public void SetRichPresence(string status) { }
    }
}
