#nullable enable

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Aurora.Editor
{
    public static class ProjectValidator
    {
        public const string ScriptsRoot = "Assets/_Project/Scripts";

        private static readonly string[] RequiredPackages =
        {
            "com.unity.render-pipelines.universal",
            "com.unity.inputsystem",
            "com.unity.cinemachine",
            "com.unity.localization",
            "com.unity.addressables",
            "com.unity.2d.animation",
            "com.unity.2d.psdimporter",
            "com.unity.2d.tilemap.extras",
            "com.unity.2d.spriteshape",
            "com.unity.timeline",
            "com.unity.test-framework",
            "com.unity.nuget.newtonsoft-json",
        };

        private static readonly Regex LegacyInputPattern = new Regex(
            @"\bUnityEngine\.Input\b|(?<![\w.])Input\.(Get\w*|mousePosition|touchCount|acceleration|compositionString)",
            RegexOptions.Compiled);

        [MenuItem("Aurora/Validate Project")]
        public static void ValidateProjectMenuItem()
        {
            var result = Validate();

            foreach (var issue in result.Issues)
                Debug.LogError($"[Aurora/Validate Project] {issue}");

            var message = result.Passed
                ? "All checks passed."
                : $"{result.Issues.Count} issue(s) found. See the Console for details.";

            if (result.Passed)
                Debug.Log($"[Aurora/Validate Project] {message}");

            EditorUtility.DisplayDialog("Aurora - Validate Project", message, "OK");
        }

        public static ValidationResult Validate()
        {
            var issues = new List<string>();

            var missingPackages = GetMissingPackages();
            if (missingPackages.Count > 0)
                issues.Add($"Missing required packages: {string.Join(", ", missingPackages)}");

            if (!IsInputSystemActive())
                issues.Add("Active Input Handling must be set to 'Input System Package (New)'.");

            var legacyInputFiles = FindLegacyInputUsages(ScriptsRoot);
            if (legacyInputFiles.Count > 0)
                issues.Add($"Legacy Input class usage found in: {string.Join(", ", legacyInputFiles)}");

            return new ValidationResult(issues);
        }

        public static List<string> GetMissingPackages()
        {
            var installed = new HashSet<string>(
                UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages().Select(p => p.name));

            return RequiredPackages.Where(p => !installed.Contains(p)).ToList();
        }

        public static bool IsInputSystemActive()
        {
            var projectSettingsPath = Path.Combine(Application.dataPath, "..", "ProjectSettings", "ProjectSettings.asset");
            if (!File.Exists(projectSettingsPath))
                return false;

            foreach (var line in File.ReadLines(projectSettingsPath))
            {
                var trimmed = line.Trim();
                if (!trimmed.StartsWith("activeInputHandler:"))
                    continue;

                var value = trimmed.Substring("activeInputHandler:".Length).Trim();
                return value == "1";
            }

            return false;
        }

        public static List<string> FindLegacyInputUsages(string rootPath)
        {
            var matches = new List<string>();
            if (!Directory.Exists(rootPath))
                return matches;

            foreach (var file in Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories))
            {
                var text = File.ReadAllText(file);
                if (LegacyInputPattern.IsMatch(text))
                    matches.Add(file.Replace('\\', '/'));
            }

            return matches;
        }

        public readonly struct ValidationResult
        {
            public ValidationResult(List<string> issues)
            {
                Issues = issues;
            }

            public List<string> Issues { get; }
            public bool Passed => Issues.Count == 0;
        }
    }
}
