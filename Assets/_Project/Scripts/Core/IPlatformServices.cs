#nullable enable

namespace Aurora.Core
{
    /// <summary>
    /// Thin wrapper over platform features (achievements, cloud save presence, rich presence).
    /// Implementations: NullPlatform (Editor/Windows without Steam), WebPlatform (Web builds),
    /// SteamPlatform (Aurora.Platform.Steam, F7-02).
    /// </summary>
    public interface IPlatformServices
    {
        string PlatformName { get; }
        bool SupportsAchievements { get; }
        bool SupportsCloudSave { get; }

        void UnlockAchievement(string id);
        void SetRichPresence(string status);
    }
}
