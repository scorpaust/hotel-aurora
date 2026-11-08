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
    /// Builds F1-02's room/camera/transition playground INSIDE the one persistent
    /// SCN_PrototypeSandbox, under a "RoomPlayground" root — positioned well clear of the
    /// F1-01/F1-08 sandbox content (which occupies roughly x in [-1, 17]).
    ///
    /// Unlike the old TestWingSceneBuilder (deleted), this never calls NewScene(): it only
    /// destroys and rebuilds its own "RoomPlayground" subtree, so manual tweaks made
    /// anywhere else in the sandbox survive a re-run. Re-running DOES still wipe anything
    /// manually added *inside* RoomPlayground itself — that's the one tradeoff of keeping
    /// this regenerable from code at all.
    /// </summary>
    public static class RoomPlaygroundBuilder
    {
        private const string SandboxScenePath = "Assets/Tests/Scenes/SCN_PrototypeSandbox.unity";
        private const string AnnexScenePath = "Assets/Tests/Scenes/SCN_SandboxAnnex.unity";
        private const string TomasObjectName = "PFB_TomasPreview";
        private const string FadeOutEventPath = "Assets/_Project/Data/Events/SO_RoomFadeOutRequested.asset";
        private const string FadeInEventPath = "Assets/_Project/Data/Events/SO_RoomFadeInRequested.asset";
        private const string RoomDataFolder = "Assets/_Project/Data/Rooms";
        private const string GateFlagKey = "f1_02.test.gate";
        private const string RootName = "RoomPlayground";

        private const float RoomWidth = 24f;
        private const float RoomHeight = 10f;
        private const float RoomSpacing = 30f; // room width + a 6-unit gap
        private const float OrthographicSize = 1.8f; // matches the sandbox's own Main Camera
        private const float PlaygroundOriginX = 40f; // clear of the F1-01/F1-08 sandbox (x in [-1, 17])

        private static readonly Color RoomAColor = new(0.722f, 0.541f, 0.243f); // --aurora-brass
        private static readonly Color RoomBColor = new(0.624f, 0.769f, 0.780f); // --aurora-ghost
        private static readonly Color RoomCColor = new(0.914f, 0.875f, 0.784f); // --aurora-paper
        private static readonly Color RoomDColor = new(0.478f, 0.361f, 0.165f); // --aurora-brass-dim
        private static readonly Color ConnectionColor = new(0.494f, 0.122f, 0.122f); // --aurora-blood

        // Tomás's F1-01 spawn — recorded so "Return to F1-01 Corridor" can send him back.
        private static readonly Vector3 CorridorSpawnPosition = new(0.78f, 0.02f, 0f);
        private static readonly Vector3 PlaygroundSpawnPosition = new(PlaygroundOriginX + RoomWidth / 2f, 0.78f, 0f);

        [MenuItem("Aurora/Level Generator/Go To Room Playground")]
        public static void GoToRoomPlayground()
        {
            var root = GameObject.Find(RootName);
            if (root == null)
            {
                Debug.LogError($"[RoomPlaygroundBuilder] No '{RootName}' in the active scene — run Build Room Playground first.");
                return;
            }

            var cameraTransform = root.transform.Find("Main Camera (Playground)");
            if (cameraTransform == null)
            {
                Debug.LogError("[RoomPlaygroundBuilder] 'Main Camera (Playground)' not found under RoomPlayground.");
                return;
            }

            cameraTransform.gameObject.SetActive(true);
            WarpTomas(PlaygroundSpawnPosition);
            ForceGameViewRepaint();
            Debug.Log("[RoomPlaygroundBuilder] Playground camera enabled, Tomás moved to RoomA's spawn.");
        }

        [MenuItem("Aurora/Level Generator/Return To F1-01 Corridor")]
        public static void ReturnToCorridor()
        {
            var root = GameObject.Find(RootName);
            var cameraTransform = root != null ? root.transform.Find("Main Camera (Playground)") : null;
            if (cameraTransform != null)
                cameraTransform.gameObject.SetActive(false);

            WarpTomas(CorridorSpawnPosition);
            ForceGameViewRepaint();
            Debug.Log("[RoomPlaygroundBuilder] Playground camera disabled, Tomás moved back to the F1-01 corridor spawn.");
        }

        /// <summary>Moves Tomás in either Edit or Play mode — in Play, goes through WarpTo() so the Rigidbody2D stays in sync, not just the Transform.</summary>
        private static void WarpTomas(Vector3 position)
        {
            var tomas = GameObject.Find(TomasObjectName);
            if (tomas == null)
            {
                Debug.LogError($"[RoomPlaygroundBuilder] Could not find '{TomasObjectName}' in the active scene.");
                return;
            }

            if (Application.isPlaying && tomas.TryGetComponent<PlayerController>(out var controller))
                controller.WarpTo(position);
            else
                tomas.transform.position = position;
        }

        /// <summary>
        /// The Game view in Edit mode (not Play) only redraws on certain triggers — without
        /// this, toggling the camera/moving Tomás from a menu command can leave the panel
        /// showing a stale frame from before the change until the user clicks into it or
        /// moves the mouse over it (confirmed live: this exact "nothing updates" symptom).
        /// </summary>
        private static void ForceGameViewRepaint()
        {
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
            foreach (var view in Resources.FindObjectsOfTypeAll<EditorWindow>())
            {
                if (view.GetType().Name == "GameView")
                    view.Repaint();
            }
        }

        [MenuItem("Aurora/Level Generator/Build Room Playground (F1-02)")]
        public static void Build()
        {
            if (!TryPrepareSandboxScene())
                return;

            var roomA = CreateRoomDefinition("RoomA", "Room A");
            var roomB = CreateRoomDefinition("RoomB", "Room B");
            var roomC = CreateRoomDefinition("RoomC", "Room C");
            var roomD = CreateRoomDefinition("RoomD", "Annex Room D");

            // Order matters: BuildAnnexScene() opens a new empty scene (NewScene), which
            // replaces the sandbox as Unity's active scene. Build the sandbox playground
            // first, while TryPrepareSandboxScene()'s check above still holds.
            BuildPlaygroundInSandbox(roomA, roomB, roomC);
            BuildAnnexScene(roomD);

            AddressablesSetup.Initialize();
            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.GetSettings(true);
            var group = AddressablesSetup.GetOrCreateGroup(settings, "TestWing");
            AddressablesSetup.AddSceneAsAddressable(settings, AnnexScenePath, group);
            AssetDatabase.SaveAssets();

            // BuildAnnexScene() leaves the Annex scene active (it has to, to save it) — switch
            // back to the sandbox so the Editor ends up where the user actually works.
            EditorSceneManager.OpenScene(SandboxScenePath, OpenSceneMode.Single);

            Debug.Log("[RoomPlaygroundBuilder] Build complete: RoomPlayground inside SCN_PrototypeSandbox + SCN_SandboxAnnex.");
        }

        /// <summary>Opens the sandbox as the active scene if it isn't already, refusing if that would discard unsaved work.</summary>
        private static bool TryPrepareSandboxScene()
        {
            var active = EditorSceneManager.GetActiveScene();

            if (active.path == SandboxScenePath)
            {
                if (active.isDirty)
                {
                    Debug.LogError("[RoomPlaygroundBuilder] SCN_PrototypeSandbox has unsaved changes — save first so a rebuild can't lose them.");
                    return false;
                }

                return true;
            }

            if (active.isDirty)
            {
                Debug.LogError($"[RoomPlaygroundBuilder] Active scene '{active.name}' has unsaved changes — save or discard before switching to SCN_PrototypeSandbox.");
                return false;
            }

            EditorSceneManager.OpenScene(SandboxScenePath, OpenSceneMode.Single);
            return true;
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

        // The Annex loads additively ON TOP of the sandbox (that's the real gameplay pattern
        // for an ala scene) — world space is shared, so its room must sit well clear of both
        // the F1-01 content (x in [-1, 17]) and the playground (x in [40, 130]-ish), or they
        // render overlapped on top of each other (confirmed live: origin (0,0) rendered
        // straight through the sandbox's own Tilemap walls).
        private const float AnnexOriginX = 200f;

        private static void BuildAnnexScene(RoomDefinition roomD)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateRoom(roomD, new Vector2(AnnexOriginX, 0f), "Spawn_D", RoomDColor, out var roomObjD);

            var stairwellBack = CreateConnectionZoneAt(roomObjD.transform, "Stairwell_Back", new Vector2(0.5f, 1.3f));
            stairwellBack.Configure(
                RoomConnectionType.Stairwell,
                new RoomDestination("RoomA", "Spawn_A"));

            EditorSceneManager.SaveScene(scene, AnnexScenePath);
        }

        private static void BuildPlaygroundInSandbox(RoomDefinition roomA, RoomDefinition roomB, RoomDefinition roomC)
        {
            var existingRoot = GameObject.Find(RootName);
            if (existingRoot != null)
                Object.DestroyImmediate(existingRoot);

            var root = new GameObject(RootName);

            CreateRoom(roomA, new Vector2(PlaygroundOriginX, 0f), "Spawn_A", RoomAColor, out var roomObjA);
            roomObjA.transform.SetParent(root.transform);
            var doorConditional = CreateConnectionZoneAt(roomObjA.transform, "Door_Conditional", new Vector2(RoomWidth - 0.5f, 1.3f));
            doorConditional.Configure(
                RoomConnectionType.Door,
                new RoomDestination("RoomB", "Spawn_B"),
                GateFlagKey, 1,
                new RoomDestination("RoomC", "Spawn_C"));

            CreateRoom(roomB, new Vector2(PlaygroundOriginX + RoomSpacing, 0f), "Spawn_B", RoomBColor, out var roomObjB);
            roomObjB.transform.SetParent(root.transform);
            var doorBackB = CreateConnectionZoneAt(roomObjB.transform, "Door_Back", new Vector2(0.5f, 1.3f));
            doorBackB.Configure(RoomConnectionType.Door, new RoomDestination("RoomA", "Spawn_A"));

            CreateRoom(roomC, new Vector2(PlaygroundOriginX + RoomSpacing * 2f, 0f), "Spawn_C", RoomCColor, out var roomObjC);
            roomObjC.transform.SetParent(root.transform);
            var doorBackC = CreateConnectionZoneAt(roomObjC.transform, "Door_Back", new Vector2(0.5f, 1.3f));
            doorBackC.Configure(RoomConnectionType.Door, new RoomDestination("RoomA", "Spawn_A"));

            var stairwellToAnnex = CreateConnectionZoneAt(roomObjC.transform, "Stairwell_ToAnnex", new Vector2(RoomWidth - 0.5f, 1.3f));
            stairwellToAnnex.Configure(
                RoomConnectionType.Stairwell,
                new RoomDestination("RoomD", "Spawn_D", "SCN_SandboxAnnex"));

            // Reuse the ONE existing Tomás already in the sandbox (from F1-01) — never
            // instantiate a second one, or Play mode ends up with two PlayerControllers both
            // reading the same input and moving two different bodies at once. To test rooms,
            // drag/position that Tomás into the playground area (e.g. around
            // (PlaygroundOriginX + RoomWidth/2, 0.78)) before or during Play; leave him at his
            // F1-01 spawn to test the corridor instead — same scene, same character, either
            // test area.
            var tomas = GameObject.Find(TomasObjectName);
            if (tomas == null)
            {
                Debug.LogError($"[RoomPlaygroundBuilder] Could not find '{TomasObjectName}' in the sandbox — is the F1-01 content still there?");
                return;
            }

            var cameraGo = new GameObject("Main Camera (Playground)");
            cameraGo.transform.SetParent(root.transform);
            var camera = cameraGo.AddComponent<Camera>();
            camera.tag = "Untagged"; // the sandbox already has its own "MainCamera"-tagged camera
            camera.depth = 10; // render on top of the sandbox's own Main Camera (depth 0)
            camera.orthographic = true;
            camera.orthographicSize = OrthographicSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.106f, 0.09f, 0.078f, 1f); // --aurora-ink
            // No AudioListener here — the sandbox's existing Main Camera already has the only
            // one this scene needs; a second one logs Unity's "2 audio listeners" warning.
            var brain = cameraGo.AddComponent<CinemachineBrain>();
            // PlayerController moves its kinematic Rigidbody2D from Update(), not FixedUpdate() —
            // SmartUpdate's heuristic (which picks the update phase based on how the Follow
            // target seems to move) got this wrong and the camera never caught up at all.
            brain.UpdateMethod = CinemachineBrain.UpdateMethods.LateUpdate;

            var vcamGo = new GameObject("CM_RoomCamera");
            vcamGo.transform.SetParent(root.transform);
            var vcam = vcamGo.AddComponent<CinemachineCamera>();
            vcam.Follow = tomas.transform;
            var lens = vcam.Lens;
            lens.ModeOverride = LensSettings.OverrideModes.Orthographic;
            lens.OrthographicSize = OrthographicSize;
            vcam.Lens = lens;
            var composer = vcamGo.AddComponent<CinemachinePositionComposer>();
            // Reset() defaults Damping to (1,1,1) — a ~1s time constant that reads as "not
            // following" over a short test.
            composer.Damping = new Vector3(0.15f, 0.3f, 0f);
            var confiner = vcamGo.AddComponent<CinemachineConfiner2D>();
            confiner.BoundingShape2D = roomObjA.GetComponent<Collider2D>();

            var fadeOut = AssetDatabase.LoadAssetAtPath<GameEvent>(FadeOutEventPath);
            var fadeIn = AssetDatabase.LoadAssetAtPath<GameEvent>(FadeInEventPath);

            var transitionGo = new GameObject("RoomTransitionController");
            transitionGo.transform.SetParent(root.transform);
            var transitionController = transitionGo.AddComponent<RoomTransitionController>();
            transitionController.Configure(confiner, fadeOut, fadeIn, 0.25f);

            var fadeUiGo = new GameObject("RoomFadeUI");
            fadeUiGo.transform.SetParent(root.transform);
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

            // Off by default: its depth (10) renders over the sandbox's own Main Camera
            // regardless of where Tomás actually is, so with it active the F1-01 corridor
            // test (Tomás left at his usual spawn, nowhere near the playground) just shows an
            // empty RoomA — confirmed live, this is what the "only black and brown" report
            // was. Tick this GameObject on in the Inspector only while testing rooms.
            cameraGo.SetActive(false);

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        private static void CreateRoom(RoomDefinition definition, Vector2 origin, string spawnId, Color color, out GameObject roomObject)
        {
            roomObject = new GameObject($"Room_{definition.Id}");
            roomObject.transform.position = origin;

            // 24x10 — the camera's view at orthographic size 1.8 in 16:9 is only ~6.4x3.6. A
            // room much smaller than that leaves CinemachineConfiner2D almost no room to pan
            // before it legitimately clamps at the walls, which reads as "camera locks up"
            // even though it's working correctly (confirmed live at 6x4 and 10x6).
            var bounds = roomObject.AddComponent<BoxCollider2D>();
            bounds.isTrigger = true;
            bounds.size = new Vector2(RoomWidth, RoomHeight);
            bounds.offset = new Vector2(RoomWidth / 2f, RoomHeight / 2f);

            var room = roomObject.AddComponent<Room>();
            room.Configure(definition, bounds);

            // Visible floor strip so the room's extent and "ground" read at a glance — no more
            // guessing at invisible colliders (the previous version had none of this, hence the
            // need to drop in manual squares just to see what was going on).
            CreateColoredQuad("Visual_Floor", roomObject.transform, new Vector2(RoomWidth / 2f, 0.4f), new Vector2(RoomWidth, 1.6f), color, 0.6f);

            // Horizontally centered, not corner-hugging — near an edge, the confiner correctly
            // clamps the camera away from the target to avoid showing outside the room, which
            // looks like "not following" if that's the very first thing a player sees.
            var spawnGo = new GameObject($"Spawn_{definition.Id}");
            spawnGo.transform.SetParent(roomObject.transform);
            spawnGo.transform.localPosition = new Vector3(RoomWidth / 2f, 0.78f, 0f);
            var spawn = spawnGo.AddComponent<RoomSpawnPoint>();
            spawn.SetId(spawnId);
        }

        private static RoomConnection CreateConnectionZoneAt(Transform roomParent, string name, Vector2 localPosition)
        {
            var go = new GameObject(name);
            go.transform.SetParent(roomParent);
            go.transform.localPosition = localPosition;
            var collider = go.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            var size = new Vector2(0.6f, 1.6f);
            collider.size = size;

            CreateColoredQuad("Visual", go.transform, Vector2.zero, size, ConnectionColor, 1f);

            return go.AddComponent<RoomConnection>();
        }

        /// <summary>A solid-color rectangle with no external art dependency: a 1x1 white sprite at 1 pixel-per-unit, tinted and scaled.</summary>
        private static void CreateColoredQuad(string name, Transform parent, Vector2 localPosition, Vector2 size, Color color, float alpha)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localPosition = localPosition;
            go.transform.localScale = new Vector3(size.x, size.y, 1f);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = GetOrCreateWhiteSprite();
            renderer.color = new Color(color.r, color.g, color.b, alpha);
            renderer.sortingOrder = -10;
        }

        private static Sprite? _whiteSprite;

        private static Sprite GetOrCreateWhiteSprite()
        {
            if (_whiteSprite != null)
                return _whiteSprite;

            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false) { name = "WhiteSquare" };
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            _whiteSprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
            return _whiteSprite;
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
