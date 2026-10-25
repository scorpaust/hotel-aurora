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

            CreateRoom(roomA, new Vector2(0f, 0f), "Spawn_A", out var roomObjA);
            var doorConditional = CreateConnectionZoneAt(roomObjA.transform, "Door_Conditional", new Vector2(5.5f, 1.3f));
            doorConditional.Configure(
                RoomConnectionType.Door,
                new RoomDestination("RoomB", "Spawn_B"),
                GateFlagKey, 1,
                new RoomDestination("RoomC", "Spawn_C"));

            CreateRoom(roomB, new Vector2(10f, 0f), "Spawn_B", out var roomObjB);
            var doorBackB = CreateConnectionZoneAt(roomObjB.transform, "Door_Back", new Vector2(0.5f, 1.3f));
            doorBackB.Configure(RoomConnectionType.Door, new RoomDestination("RoomA", "Spawn_A"));

            CreateRoom(roomC, new Vector2(20f, 0f), "Spawn_C", out var roomObjC);
            var doorBackC = CreateConnectionZoneAt(roomObjC.transform, "Door_Back", new Vector2(0.5f, 1.3f));
            doorBackC.Configure(RoomConnectionType.Door, new RoomDestination("RoomA", "Spawn_A"));

            var stairwellToAnnex = CreateConnectionZoneAt(roomObjC.transform, "Stairwell_ToAnnex", new Vector2(5.5f, 1.3f));
            stairwellToAnnex.Configure(
                RoomConnectionType.Stairwell,
                new RoomDestination("RoomD", "Spawn_D", "SCN_TestWingAnnex"));

            var tomasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(TomasPrefabPath);
            var tomas = (GameObject)PrefabUtility.InstantiatePrefab(tomasPrefab);
            tomas.transform.position = new Vector3(1f, 0.78f, 0f);

            var cameraGo = new GameObject("Main Camera");
            var camera = cameraGo.AddComponent<Camera>();
            camera.tag = "MainCamera";
            cameraGo.AddComponent<AudioListener>();
            cameraGo.AddComponent<CinemachineBrain>();

            var vcamGo = new GameObject("CM_RoomCamera");
            var vcam = vcamGo.AddComponent<CinemachineCamera>();
            vcam.Follow = tomas.transform;
            vcamGo.AddComponent<CinemachinePositionComposer>();
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

            var bounds = roomObject.AddComponent<BoxCollider2D>();
            bounds.isTrigger = true;
            bounds.size = new Vector2(6f, 4f);
            bounds.offset = new Vector2(3f, 2f);

            var room = roomObject.AddComponent<Room>();
            room.Configure(definition, bounds);

            var spawnGo = new GameObject($"Spawn_{definition.Id}");
            spawnGo.transform.SetParent(roomObject.transform);
            spawnGo.transform.localPosition = new Vector3(1f, 0.78f, 0f);
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
