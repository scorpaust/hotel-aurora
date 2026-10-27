#nullable enable

using Aurora.Core;
using Aurora.Gameplay;
using Aurora.Hotel;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aurora.Editor
{
    /// <summary>
    /// One-shot, idempotent builder for F1-02's test content: SCN_TestWing (3 rooms: a
    /// conditional door A to B/C, both with a Door back to A) plus SCN_TestWingAnnex (1
    /// room, reached from C via a Stairwell) to exercise the ala-crossing scene load.
    /// Placeholder geometry only — no Tilemap/art, see F1-02's Registo for why.
    /// </summary>
    public static class TestWingSceneBuilder
    {
        private const string TestWingScenePath = "Assets/Tests/Scenes/SCN_TestWing.unity";
        private const string AnnexScenePath = "Assets/Tests/Scenes/SCN_TestWingAnnex.unity";
        private const string TomasPrefabPath = "Assets/_Project/Prefabs/Characters/PFB_TomasPreview.prefab";
        private const string FadeOutEventPath = "Assets/_Project/Data/Events/SO_RoomFadeOutRequested.asset";
        private const string FadeInEventPath = "Assets/_Project/Data/Events/SO_RoomFadeInRequested.asset";
        private const string RoomDataFolder = "Assets/_Project/Data/Rooms";
        private const string GateFlagKey = "f1_02.test.gate";

        [MenuItem("Aurora/Level Generator/Build F1-02 Test Wing")]
        public static void Build()
        {
            var roomD = CreateRoomDefinition("RoomD", "Annex Room D");
            BuildAnnexScene(roomD);

            var roomA = CreateRoomDefinition("RoomA", "Room A");
            var roomB = CreateRoomDefinition("RoomB", "Room B");
            var roomC = CreateRoomDefinition("RoomC", "Room C");
            BuildTestWingScene(roomA, roomB, roomC);

            AddressablesSetup.Initialize();
            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.GetSettings(true);
            var group = AddressablesSetup.GetOrCreateGroup(settings, "TestWing");
            AddressablesSetup.AddSceneAsAddressable(settings, AnnexScenePath, group);
            AssetDatabase.SaveAssets();

            Debug.Log("[TestWingSceneBuilder] Build complete: SCN_TestWing + SCN_TestWingAnnex.");
        }

        private static RoomDefinition CreateRoomDefinition(string id, string displayName)
        {
            var path = $"{RoomDataFolder}/SO_{id}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<RoomDefinition>(path);
            if (existing != null)
                return existing;

            var definition = ScriptableObject.CreateInstance<RoomDefinition>();
            definition.Configure(id, displayName);
            AssetDatabase.CreateAsset(definition, path);
            return definition;
        }

        private static void BuildAnnexScene(RoomDefinition roomD)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateRoom(roomD, new Vector2(0f, 0f), "Spawn_D", out _);

            var stairwellBack = CreateConnectionZone("Stairwell_Back", new Vector2(0.5f, 1.3f));
            stairwellBack.Configure(
                RoomConnectionType.Stairwell,
                new RoomDestination("RoomA", "Spawn_A"));

            EditorSceneManager.SaveScene(scene, AnnexScenePath);
        }

        private static void BuildTestWingScene(RoomDefinition roomA, RoomDefinition roomB, RoomDefinition roomC)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            const float roomSpacing = 30f; // room width (24) + a 6-unit gap between rooms

            CreateRoom(roomA, new Vector2(0f, 0f), "Spawn_A", out var roomObjA);
            var doorConditional = CreateConnectionZoneAt(roomObjA.transform, "Door_Conditional", new Vector2(23.5f, 1.3f));
            doorConditional.Configure(
                RoomConnectionType.Door,
                new RoomDestination("RoomB", "Spawn_B"),
                GateFlagKey, 1,
                new RoomDestination("RoomC", "Spawn_C"));

            CreateRoom(roomB, new Vector2(roomSpacing, 0f), "Spawn_B", out var roomObjB);
            var doorBackB = CreateConnectionZoneAt(roomObjB.transform, "Door_Back", new Vector2(0.5f, 1.3f));
            doorBackB.Configure(RoomConnectionType.Door, new RoomDestination("RoomA", "Spawn_A"));

            CreateRoom(roomC, new Vector2(roomSpacing * 2f, 0f), "Spawn_C", out var roomObjC);
            var doorBackC = CreateConnectionZoneAt(roomObjC.transform, "Door_Back", new Vector2(0.5f, 1.3f));
            doorBackC.Configure(RoomConnectionType.Door, new RoomDestination("RoomA", "Spawn_A"));

            var stairwellToAnnex = CreateConnectionZoneAt(roomObjC.transform, "Stairwell_ToAnnex", new Vector2(23.5f, 1.3f));
            stairwellToAnnex.Configure(
                RoomConnectionType.Stairwell,
                new RoomDestination("RoomD", "Spawn_D", "SCN_TestWingAnnex"));

            var tomasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(TomasPrefabPath);
            var tomas = (GameObject)PrefabUtility.InstantiatePrefab(tomasPrefab);
            tomas.transform.position = new Vector3(12f, 0.78f, 0f);

            // Matches the project's 2D orthographic convention (see SCN_PrototypeSandbox's Main
            // Camera) — both default to Perspective otherwise, which also breaks
            // CinemachineConfiner2D (it's built for an orthographic 2D camera; under Perspective
            // it degenerates and snaps the camera to the bounding shape's centroid instead of
            // following anything).
            const float orthographicSize = 1.8f;

            var cameraGo = new GameObject("Main Camera");
            var camera = cameraGo.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.orthographic = true;
            camera.orthographicSize = orthographicSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.106f, 0.09f, 0.078f, 1f); // --aurora-ink
            cameraGo.AddComponent<AudioListener>();
            var brain = cameraGo.AddComponent<CinemachineBrain>();
            // PlayerController moves its kinematic Rigidbody2D from Update(), not FixedUpdate() —
            // SmartUpdate's heuristic (which picks the update phase based on how the Follow
            // target seems to move) got this wrong and the camera never caught up at all.
            // LateUpdate is also Cinemachine's own general recommendation outside of
            // physics-interpolated rigidbodies.
            brain.UpdateMethod = CinemachineBrain.UpdateMethods.LateUpdate;

            var vcamGo = new GameObject("CM_RoomCamera");
            var vcam = vcamGo.AddComponent<CinemachineCamera>();
            vcam.Follow = tomas.transform;
            var lens = vcam.Lens;
            lens.ModeOverride = LensSettings.OverrideModes.Orthographic;
            lens.OrthographicSize = orthographicSize;
            vcam.Lens = lens;
            var composer = vcamGo.AddComponent<CinemachinePositionComposer>();
            // Reset() defaults Damping to (1,1,1) — a ~1s time constant that reads as "the
            // camera isn't following at all" during normal walking (confirmed live: it does
            // catch up, just 2+ seconds late). Spec item 4 asks for a responsive camera
            // ("dead zone horizontal pequena; lookahead leve"), so tighten it here.
            composer.Damping = new Vector3(0.15f, 0.3f, 0f);
            var confiner = vcamGo.AddComponent<CinemachineConfiner2D>();
            confiner.BoundingShape2D = roomObjA.GetComponent<Collider2D>();

            var fadeOut = AssetDatabase.LoadAssetAtPath<GameEvent>(FadeOutEventPath);
            var fadeIn = AssetDatabase.LoadAssetAtPath<GameEvent>(FadeInEventPath);

            var transitionGo = new GameObject("RoomTransitionController");
            var transitionController = transitionGo.AddComponent<RoomTransitionController>();
            transitionController.Configure(confiner, fadeOut, fadeIn, 0.25f);

            var fadeUiGo = new GameObject("RoomFadeUI");
            var uiDocument = fadeUiGo.AddComponent<UIDocument>();
            uiDocument.panelSettings = GetOrCreateFadePanelSettings();
            fadeUiGo.AddComponent<Aurora.UI.RoomFadePanel>();

            var fadePanel = fadeUiGo.GetComponent<Aurora.UI.RoomFadePanel>();

            var listenerOutGo = new GameObject("Listener_FadeOut");
            listenerOutGo.transform.SetParent(fadeUiGo.transform);
            var listenerOut = listenerOutGo.AddComponent<GameEventListener>();
            listenerOut.SetEvent(fadeOut);
            UnityEventTools.AddVoidPersistentListener(listenerOut.Response, fadePanel.FadeOut);

            var listenerInGo = new GameObject("Listener_FadeIn");
            listenerInGo.transform.SetParent(fadeUiGo.transform);
            var listenerIn = listenerInGo.AddComponent<GameEventListener>();
            listenerIn.SetEvent(fadeIn);
            UnityEventTools.AddVoidPersistentListener(listenerIn.Response, fadePanel.FadeIn);

            EditorSceneManager.SaveScene(scene, TestWingScenePath);
        }

        private static void CreateRoom(RoomDefinition definition, Vector2 origin, string spawnId, out GameObject roomObject)
        {
            roomObject = new GameObject($"Room_{definition.Id}");
            roomObject.transform.position = origin;

            // 24x10 — the camera's view at orthographic size 1.8 in 16:9 is only ~6.4x3.6, so
            // 10x6 (tried first) left just a ~3.6-unit pannable window before the confiner
            // legitimately clamped at the walls — technically correct, but it reads as "camera
            // locks up" since most of the room is outside that window. 24x10 gives ~17.6 units
            // of real horizontal pan room.
            var bounds = roomObject.AddComponent<BoxCollider2D>();
            bounds.isTrigger = true;
            bounds.size = new Vector2(24f, 10f);
            bounds.offset = new Vector2(12f, 5f);

            var room = roomObject.AddComponent<Room>();
            room.Configure(definition, bounds);

            // Horizontally centered, not corner-hugging — near an edge, the confiner correctly
            // clamps the camera away from the target to avoid showing outside the room, which
            // looks like "not following" if that's the very first thing a player sees.
            var spawnGo = new GameObject($"Spawn_{definition.Id}");
            spawnGo.transform.SetParent(roomObject.transform);
            spawnGo.transform.localPosition = new Vector3(12f, 0.78f, 0f);
            var spawn = spawnGo.AddComponent<RoomSpawnPoint>();
            spawn.SetId(spawnId);
        }

        private static RoomConnection CreateConnectionZone(string name, Vector2 localPosition)
        {
            var go = new GameObject(name);
            go.transform.position = localPosition;
            var collider = go.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(0.6f, 1.6f);
            return go.AddComponent<RoomConnection>();
        }

        private static RoomConnection CreateConnectionZoneAt(Transform roomParent, string name, Vector2 localPosition)
        {
            var go = new GameObject(name);
            go.transform.SetParent(roomParent);
            go.transform.localPosition = localPosition;
            var collider = go.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(0.6f, 1.6f);
            return go.AddComponent<RoomConnection>();
        }

        private static PanelSettings GetOrCreateFadePanelSettings()
        {
            const string path = "Assets/Tests/UI/SO_RoomFadePanelSettings.asset";
            var existing = AssetDatabase.LoadAssetAtPath<PanelSettings>(path);
            if (existing != null)
                return existing;

            var settings = ScriptableObject.CreateInstance<PanelSettings>();
            AssetDatabase.CreateAsset(settings, path);
            return settings;
        }
    }
}
