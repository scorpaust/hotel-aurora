#nullable enable

using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Aurora.Editor
{
    /// <summary>
    /// One-off, idempotent setup for Addressables (F1-02 spec item 5: ISceneService loads
    /// ala scenes via Addressables.LoadSceneAsync, grouped by ala). Creates the default
    /// settings if missing and marks the given scenes addressable, keyed by their own
    /// filename so `Addressables.LoadSceneAsync("SceneName", ...)` works exactly like the
    /// old bare SceneManager name did.
    /// </summary>
    public static class AddressablesSetup
    {
        private const string CoreGroupName = "Core";
        private const string MainMenuScenePath = "Assets/_Project/Scenes/MainMenu.unity";

        [MenuItem("Aurora/Addressables/Initialize")]
        public static void Initialize()
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            if (settings == null)
            {
                Debug.LogError("[AddressablesSetup] Could not create/load AddressableAssetSettings.");
                return;
            }

            var coreGroup = GetOrCreateGroup(settings, CoreGroupName);
            AddSceneAsAddressable(settings, MainMenuScenePath, coreGroup);

            AssetDatabase.SaveAssets();
            Debug.Log("[AddressablesSetup] Initialize complete.");
        }

        public static AddressableAssetGroup GetOrCreateGroup(AddressableAssetSettings settings, string groupName)
        {
            var existing = settings.groups.FirstOrDefault(g => g != null && g.Name == groupName);
            if (existing != null)
                return existing;

            return settings.CreateGroup(groupName, false, false, true, null,
                typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema),
                typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.ContentUpdateGroupSchema));
        }

        public static void AddSceneAsAddressable(AddressableAssetSettings settings, string scenePath, AddressableAssetGroup group)
        {
            var guid = AssetDatabase.AssetPathToGUID(scenePath);
            if (string.IsNullOrEmpty(guid))
            {
                Debug.LogError($"[AddressablesSetup] Scene not found: {scenePath}");
                return;
            }

            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        }
    }
}
