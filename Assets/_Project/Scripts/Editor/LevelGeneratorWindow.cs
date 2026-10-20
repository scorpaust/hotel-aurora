#nullable enable

using System.IO;
using System.Linq;
using Aurora.Hotel;
using UnityEditor;
using UnityEngine;

namespace Aurora.Editor
{
    /// <summary>
    /// Assisted scene scaffolding from a LevelFlowDefinition: instantiates one placeholder
    /// per beat, in order, with configurable spacing. Never decides which content a beat
    /// uses beyond what the LevelFlowDefinition asset already says, and never runs outside
    /// the Editor — it only ever touches the currently open scene.
    /// </summary>
    public sealed class LevelGeneratorWindow : EditorWindow
    {
        private const string GeneratedRootName = "LevelFlow_Generated";
        private const string PlaceholderSpritePath = "Assets/_Project/Art/Placeholders/SPR_LevelBeatPlaceholder.png";

        private LevelFlowDefinition?[] _flows = System.Array.Empty<LevelFlowDefinition?>();
        private string[] _flowLabels = System.Array.Empty<string>();
        private int _selectedFlowIndex;
        private RoomDefinition? _targetRoom;
        private float _spacing = 3f;

        [MenuItem("Aurora/Level Generator")]
        public static void Open()
        {
            GetWindow<LevelGeneratorWindow>("Level Generator");
        }

        private void OnEnable()
        {
            RefreshFlows();
        }

        private void RefreshFlows()
        {
            _flows = AssetDatabase.FindAssets("t:LevelFlowDefinition")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<LevelFlowDefinition>)
                .ToArray();

            _flowLabels = _flows
                .Select(flow => string.IsNullOrWhiteSpace(flow?.Id) ? "(sem id)" : flow!.Id)
                .ToArray();

            if (_selectedFlowIndex >= _flows.Length)
                _selectedFlowIndex = 0;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Level flows", EditorStyles.boldLabel);

            if (_flows.Length == 0)
            {
                EditorGUILayout.HelpBox("No LevelFlowDefinition assets found in the project.", MessageType.Info);
            }
            else
            {
                _selectedFlowIndex = EditorGUILayout.Popup("Flow", _selectedFlowIndex, _flowLabels);
            }

            if (GUILayout.Button("Refresh list"))
                RefreshFlows();

            EditorGUILayout.Space();
            _targetRoom = (RoomDefinition?)EditorGUILayout.ObjectField(
                "Target room (optional)", _targetRoom, typeof(RoomDefinition), false);
            _spacing = EditorGUILayout.FloatField("Beat spacing", _spacing);

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(_flows.Length == 0))
            {
                if (GUILayout.Button("Generate"))
                    Generate(_flows[_selectedFlowIndex], _spacing);
            }
        }

        private static void Generate(LevelFlowDefinition? flow, float spacing)
        {
            if (flow == null)
            {
                EditorUtility.DisplayDialog("Aurora - Level Generator", "No flow selected.", "OK");
                return;
            }

            var validation = LevelFlowValidator.Validate(flow);
            if (!validation.Passed)
            {
                foreach (var issue in validation.Issues)
                    Debug.LogError($"[Aurora/Level Generator] {issue}");

                EditorUtility.DisplayDialog(
                    "Aurora - Level Generator",
                    $"{validation.Issues.Count} issue(s) with '{flow.Id}'. See the Console for details.",
                    "OK");
                return;
            }

            var existingRoot = GameObject.Find(GeneratedRootName);
            if (existingRoot != null)
                Undo.DestroyObjectImmediate(existingRoot);

            var root = new GameObject(GeneratedRootName);
            Undo.RegisterCreatedObjectUndo(root, "Generate Level Flow");

            var placeholderSprite = EnsurePlaceholderSprite();

            for (var i = 0; i < flow.Beats.Count; i++)
            {
                var beat = flow.Beats[i];
                var beatName = string.IsNullOrWhiteSpace(beat.Label)
                    ? $"Beat_{i:00}_{beat.Type}"
                    : $"Beat_{i:00}_{beat.Type}_{beat.Label}";

                var beatObject = new GameObject(beatName);
                beatObject.transform.SetParent(root.transform);
                beatObject.transform.localPosition = new Vector3(i * Mathf.Max(0.1f, spacing), 0f, 0f);

                var renderer = beatObject.AddComponent<SpriteRenderer>();
                renderer.sprite = placeholderSprite;
                renderer.color = BeatColor(beat.Type);
            }

            Debug.Log($"[Aurora/Level Generator] Generated {flow.Beats.Count} beat(s) from '{flow.Id}'.");
        }

        private static Color BeatColor(LevelBeatType type) => type switch
        {
            LevelBeatType.Cutscene => new Color(0.624f, 0.769f, 0.780f), // aurora-ghost
            LevelBeatType.Exploration => new Color(0.722f, 0.549f, 0.243f), // aurora-brass
            LevelBeatType.EnemyEncounter => new Color(0.494f, 0.122f, 0.122f), // aurora-blood
            LevelBeatType.TrapGauntlet => new Color(0.722f, 0.345f, 0.133f),
            LevelBeatType.Maze => new Color(0.478f, 0.361f, 0.165f), // aurora-brass-dim
            LevelBeatType.NarrativeInteraction => new Color(0.890f, 0.875f, 0.784f), // aurora-paper
            LevelBeatType.MiniBoss => new Color(0.624f, 0.169f, 0.169f),
            LevelBeatType.BonusLevel => new Color(1f, 0.890f, 0.639f), // aurora-light
            LevelBeatType.Boss => new Color(0.333f, 0.059f, 0.059f),
            _ => Color.gray
        };

        private static Sprite EnsurePlaceholderSprite()
        {
            var existing = AssetDatabase.LoadAssetAtPath<Sprite>(PlaceholderSpritePath);
            if (existing != null)
                return existing;

            var directory = Path.GetDirectoryName(PlaceholderSpritePath)!;
            if (!AssetDatabase.IsValidFolder(directory))
            {
                var parent = Path.GetDirectoryName(directory)!.Replace('\\', '/');
                var folderName = Path.GetFileName(directory);
                AssetDatabase.CreateFolder(parent, folderName);
            }

            const int size = 32;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color32[size * size];
            var white = new Color32(255, 255, 255, 255);
            for (var i = 0; i < pixels.Length; i++)
                pixels[i] = white;
            texture.SetPixels32(pixels);
            texture.Apply();

            File.WriteAllBytes(PlaceholderSpritePath, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);

            AssetDatabase.ImportAsset(PlaceholderSpritePath);

            var importer = (TextureImporter)AssetImporter.GetAtPath(PlaceholderSpritePath);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 100;
            importer.filterMode = FilterMode.Point;
            importer.mipmapEnabled = false;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.SaveAndReimport();

            return AssetDatabase.LoadAssetAtPath<Sprite>(PlaceholderSpritePath);
        }
    }
}
