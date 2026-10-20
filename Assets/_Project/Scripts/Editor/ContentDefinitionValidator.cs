#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Aurora.Core;
using UnityEditor;
using UnityEngine;

namespace Aurora.Editor
{
    public static class ContentDefinitionValidator
    {
        [MenuItem("Aurora/Validate Content Definitions")]
        public static void ValidateMenuItem()
        {
            var result = Validate();

            foreach (var issue in result.Issues)
                Debug.LogError($"[Aurora/Validate Content Definitions] {issue}");

            var message = result.Passed
                ? "All checks passed."
                : $"{result.Issues.Count} issue(s) found. See the Console for details.";

            if (result.Passed)
                Debug.Log($"[Aurora/Validate Content Definitions] {message}");

            EditorUtility.DisplayDialog("Aurora - Validate Content Definitions", message, "OK");
        }

        public static ValidationResult Validate(IEnumerable<(string path, IContentDefinition definition)>? candidates = null)
        {
            candidates ??= FindAllContentDefinitions();

            var issues = new List<string>();
            var byType = new Dictionary<Type, List<(string path, IContentDefinition definition)>>();

            foreach (var candidate in candidates)
            {
                var type = candidate.definition.GetType();
                if (!byType.TryGetValue(type, out var list))
                {
                    list = new List<(string, IContentDefinition)>();
                    byType[type] = list;
                }

                list.Add(candidate);

                if (string.IsNullOrWhiteSpace(candidate.definition.Id))
                    issues.Add($"{type.Name} at '{candidate.path}' has an empty Id.");
            }

            foreach (var (type, list) in byType)
            {
                var duplicates = list
                    .Where(entry => !string.IsNullOrWhiteSpace(entry.definition.Id))
                    .GroupBy(entry => entry.definition.Id)
                    .Where(group => group.Count() > 1);

                foreach (var group in duplicates)
                {
                    var paths = string.Join(", ", group.Select(entry => entry.path));
                    issues.Add($"Duplicate {type.Name} id '{group.Key}': {paths}");
                }
            }

            return new ValidationResult(issues);
        }

        private static IEnumerable<(string, IContentDefinition)> FindAllContentDefinitions()
        {
            foreach (var guid in AssetDatabase.FindAssets("t:ScriptableObject"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (asset is IContentDefinition definition)
                    yield return (path, definition);
            }
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
