#nullable enable

using System.Linq;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEngine;

namespace Aurora.Editor.Build
{
    /// <summary>
    /// One-off creation of the project's Build Profile assets (Web, Windows, Windows-Steam).
    /// Safe to re-run: skips any profile that already exists under Assets/Settings/Build Profiles.
    /// </summary>
    public static class BuildProfileSetup
    {
        [MenuItem("Aurora/Build/Create Build Profiles")]
        public static void CreateProfiles()
        {
            var modules = BuildProfile.GetInstalledPlatformModules();
            var windowsGuid = FindPlatformGuid(modules, "Windows");
            var webGuid = FindPlatformGuid(modules, "Web");

            CreateIfMissing("Web", webGuid, null);
            CreateIfMissing("Windows", windowsGuid, null);
            CreateIfMissing("Windows-Steam", windowsGuid, new[] { "STEAM_BUILD" });

            AssetDatabase.SaveAssets();
        }

        private static GUID FindPlatformGuid(System.Collections.Generic.IReadOnlyList<InstalledPlatformInfo> modules, string displayName)
        {
            foreach (var module in modules)
            {
                if (module.displayName == displayName)
                    return module.platformGuid;
            }

            throw new System.InvalidOperationException(
                $"No installed platform module named '{displayName}'. Installed: {string.Join(", ", modules.Select(m => m.displayName))}");
        }

        private static void CreateIfMissing(string profileName, GUID platformGuid, string[]? scriptingDefines)
        {
            var path = $"Assets/Settings/Build Profiles/{profileName}.asset";
            if (AssetDatabase.LoadAssetAtPath<BuildProfile>(path) != null)
            {
                Debug.Log($"[BuildProfileSetup] '{profileName}' already exists, skipping.");
                return;
            }

            var profile = BuildProfile.CreateBuildProfile(platformGuid, profileName);
            if (scriptingDefines is { Length: > 0 })
                profile.scriptingDefines = scriptingDefines;

            EditorUtility.SetDirty(profile);
            Debug.Log($"[BuildProfileSetup] Created '{profileName}' at {AssetDatabase.GetAssetPath(profile)}.");
        }
    }
}
