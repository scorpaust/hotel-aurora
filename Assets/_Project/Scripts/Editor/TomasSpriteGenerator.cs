#nullable enable

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Aurora.Editor
{
    public static class TomasSpriteGenerator
    {
        // Palette:
        // ' ' = Transparent (0,0,0,0)
        // '#' = Black #000000 (1px outline, pupils, soles, deep creases)
        // 'D' = Dark Violet #382843 (Dark clothing/hair shadow, pants)
        // 'M' = Mid Mauve #7C6D80 (Hoodie, backpack, midtone dither)
        // '.' = Pale Silver #C7C6C6 (Skin, shoe highlights, zipper/straps)

        private static readonly Color32 ColTrans = new Color32(0, 0, 0, 0);
        private static readonly Color32 ColBlack = new Color32(0, 0, 0, 255);
        private static readonly Color32 ColDark = new Color32(0x38, 0x28, 0x43, 255);
        private static readonly Color32 ColMauve = new Color32(0x7C, 0x6D, 0x80, 255);
        private static readonly Color32 ColLight = new Color32(0xC7, 0xC6, 0xC6, 255);

        public const int FrameWidth = 32;
        public const int FrameHeight = 48;
        public const int TotalFrames = 20;
        public const string AssetPath = "Assets/Assets/Sprites/Tomas.png";
        public const string AnimDir = "Assets/_Project/Animations/Tomas";

        public static readonly string[] FrameNames = new string[]
        {
            // Idle (0..1)
            "Tomas_Idle_0",
            "Tomas_Idle_1",
            // Walk (2..7)
            "Tomas_Walk_0",
            "Tomas_Walk_1",
            "Tomas_Walk_2",
            "Tomas_Walk_3",
            "Tomas_Walk_4",
            "Tomas_Walk_5",
            // Run (8..13)
            "Tomas_Run_0",
            "Tomas_Run_1",
            "Tomas_Run_2",
            "Tomas_Run_3",
            "Tomas_Run_4",
            "Tomas_Run_5",
            // Crouch Idle (14..15)
            "Tomas_Crouch_Idle_0",
            "Tomas_Crouch_Idle_1",
            // Crouch Walk (16..19)
            "Tomas_Crouch_Walk_0",
            "Tomas_Crouch_Walk_1",
            "Tomas_Crouch_Walk_2",
            "Tomas_Crouch_Walk_3"
        };

        [MenuItem("Aurora/Generate Tomas Sprites")]
        public static void GenerateSpritesMenuItem()
        {
            Generate();
            SetupAnimationsAndController();
            Debug.Log($"[Aurora] Generated Tomas sprite sheet and animations at {AssetPath} & {AnimDir}");
        }

        public static void Generate()
        {
            int texW = FrameWidth * TotalFrames; // 640
            int texH = FrameHeight; // 48

            Texture2D tex = new Texture2D(texW, texH, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Point;

            Color32[] clearPixels = new Color32[texW * texH];
            for (int i = 0; i < clearPixels.Length; i++) clearPixels[i] = ColTrans;
            tex.SetPixels32(clearPixels);

            string[][] frames = new string[][]
            {
                // Idle
                TomasFrameData.Idle0,
                TomasFrameData.Idle1,
                // Walk
                TomasFrameData.Walk0,
                TomasFrameData.Walk1,
                TomasFrameData.Walk2,
                TomasFrameData.Walk3,
                TomasFrameData.Walk4,
                TomasFrameData.Walk5,
                // Run
                TomasFrameData.Run0,
                TomasFrameData.Run1,
                TomasFrameData.Run2,
                TomasFrameData.Run3,
                TomasFrameData.Run4,
                TomasFrameData.Run5,
                // Crouch Idle
                TomasFrameData.CrouchIdle0,
                TomasFrameData.CrouchIdle1,
                // Crouch Walk
                TomasFrameData.CrouchWalk0,
                TomasFrameData.CrouchWalk1,
                TomasFrameData.CrouchWalk2,
                TomasFrameData.CrouchWalk3
            };

            for (int f = 0; f < TotalFrames; f++)
            {
                string[] lines = frames[f];
                int startX = f * FrameWidth;
                for (int row = 0; row < FrameHeight; row++)
                {
                    // Row 0 is top of sprite (y = 47), Row 47 is bottom (y = 0)
                    int y = (FrameHeight - 1) - row;
                    string line = lines[row];
                    for (int x = 0; x < FrameWidth; x++)
                    {
                        char c = line[x];
                        Color32 color = c switch
                        {
                            '#' => ColBlack,
                            'D' => ColDark,
                            'M' => ColMauve,
                            '.' => ColLight,
                            _ => ColTrans
                        };
                        tex.SetPixel(startX + x, y, color);
                    }
                }
            }

            tex.Apply();

            string? dir = Path.GetDirectoryName(AssetPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            byte[] pngBytes = tex.EncodeToPNG();
            File.WriteAllBytes(AssetPath, pngBytes);
            AssetDatabase.ImportAsset(AssetPath, ImportAssetOptions.ForceUpdate);

            // Configure importer
            var importer = AssetImporter.GetAtPath(AssetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.sRGBTexture = true;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }

            // Slice frames
            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
            dataProvider.InitSpriteEditorDataProvider();

            var spriteRects = new SpriteRect[TotalFrames];
            for (int i = 0; i < TotalFrames; i++)
            {
                spriteRects[i] = new SpriteRect
                {
                    name = FrameNames[i],
                    rect = new Rect(i * FrameWidth, 0, FrameWidth, FrameHeight),
                    alignment = SpriteAlignment.Custom,
                    pivot = new Vector2(0.5f, 4f / FrameHeight), // Ground line baseline at Y=4
                    border = Vector4.zero,
                    spriteID = GUID.Generate()
                };
            }

            dataProvider.SetSpriteRects(spriteRects);
            dataProvider.Apply();
            importer?.SaveAndReimport();
        }

        public static void SetupAnimationsAndController()
        {
            if (!Directory.Exists(AnimDir)) Directory.CreateDirectory(AnimDir);

            Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetPath);
            var spriteMap = subAssets.OfType<Sprite>().ToDictionary(s => s.name, s => s);

            // 1. Idle Clip (2 frames, 3 FPS)
            AnimationClip idleClip = CreateClip(new Sprite[]
            {
                spriteMap["Tomas_Idle_0"],
                spriteMap["Tomas_Idle_1"],
                spriteMap["Tomas_Idle_0"]
            }, 3, $"{AnimDir}/Tomas_Idle.anim");

            // 2. Walk Clip (6 frames, 8 FPS)
            AnimationClip walkClip = CreateClip(new Sprite[]
            {
                spriteMap["Tomas_Walk_0"],
                spriteMap["Tomas_Walk_1"],
                spriteMap["Tomas_Walk_2"],
                spriteMap["Tomas_Walk_3"],
                spriteMap["Tomas_Walk_4"],
                spriteMap["Tomas_Walk_5"],
                spriteMap["Tomas_Walk_0"]
            }, 8, $"{AnimDir}/Tomas_Walk.anim");

            // 3. Run Clip (6 frames, 12 FPS - fast sprint)
            AnimationClip runClip = CreateClip(new Sprite[]
            {
                spriteMap["Tomas_Run_0"],
                spriteMap["Tomas_Run_1"],
                spriteMap["Tomas_Run_2"],
                spriteMap["Tomas_Run_3"],
                spriteMap["Tomas_Run_4"],
                spriteMap["Tomas_Run_5"],
                spriteMap["Tomas_Run_0"]
            }, 12, $"{AnimDir}/Tomas_Run.anim");

            // 4. Crouch Idle Clip (2 frames, 3 FPS)
            AnimationClip crouchIdleClip = CreateClip(new Sprite[]
            {
                spriteMap["Tomas_Crouch_Idle_0"],
                spriteMap["Tomas_Crouch_Idle_1"],
                spriteMap["Tomas_Crouch_Idle_0"]
            }, 3, $"{AnimDir}/Tomas_Crouch_Idle.anim");

            // 5. Crouch Walk Clip (4 frames, 6 FPS)
            AnimationClip crouchWalkClip = CreateClip(new Sprite[]
            {
                spriteMap["Tomas_Crouch_Walk_0"],
                spriteMap["Tomas_Crouch_Walk_1"],
                spriteMap["Tomas_Crouch_Walk_2"],
                spriteMap["Tomas_Crouch_Walk_3"],
                spriteMap["Tomas_Crouch_Walk_0"]
            }, 6, $"{AnimDir}/Tomas_Crouch_Walk.anim");

            // 6. Setup Animator Controller
            string controllerPath = $"{AnimDir}/Tomas_Controller.controller";
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if (controller == null)
            {
                controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            }

            // Ensure parameters exist
            EnsureParameter(controller, "IsMoving", AnimatorControllerParameterType.Bool);
            EnsureParameter(controller, "IsRunning", AnimatorControllerParameterType.Bool);
            EnsureParameter(controller, "IsCrouching", AnimatorControllerParameterType.Bool);

            var rootSm = controller.layers[0].stateMachine;

            // Clear old states
            var existingStates = rootSm.states.ToList();
            foreach (var childState in existingStates)
            {
                rootSm.RemoveState(childState.state);
            }

            // Add states
            var idleState = rootSm.AddState("Idle");
            idleState.motion = idleClip;

            var walkState = rootSm.AddState("Walk");
            walkState.motion = walkClip;

            var runState = rootSm.AddState("Run");
            runState.motion = runClip;

            var crouchIdleState = rootSm.AddState("Crouch_Idle");
            crouchIdleState.motion = crouchIdleClip;

            var crouchWalkState = rootSm.AddState("Crouch_Walk");
            crouchWalkState.motion = crouchWalkClip;

            rootSm.defaultState = idleState;

            // --- Transitions from Idle ---
            // Idle -> Walk: IsMoving=true, IsRunning=false, IsCrouching=false
            AddTransition(idleState, walkState,
                (AnimatorConditionMode.If, "IsMoving"),
                (AnimatorConditionMode.IfNot, "IsRunning"),
                (AnimatorConditionMode.IfNot, "IsCrouching"));

            // Idle -> Run: IsMoving=true, IsRunning=true, IsCrouching=false
            AddTransition(idleState, runState,
                (AnimatorConditionMode.If, "IsMoving"),
                (AnimatorConditionMode.If, "IsRunning"),
                (AnimatorConditionMode.IfNot, "IsCrouching"));

            // Idle -> Crouch_Idle: IsCrouching=true, IsMoving=false
            AddTransition(idleState, crouchIdleState,
                (AnimatorConditionMode.If, "IsCrouching"),
                (AnimatorConditionMode.IfNot, "IsMoving"));

            // Idle -> Crouch_Walk: IsCrouching=true, IsMoving=true
            AddTransition(idleState, crouchWalkState,
                (AnimatorConditionMode.If, "IsCrouching"),
                (AnimatorConditionMode.If, "IsMoving"));

            // --- Transitions from Walk ---
            // Walk -> Idle: IsMoving=false, IsCrouching=false
            AddTransition(walkState, idleState,
                (AnimatorConditionMode.IfNot, "IsMoving"),
                (AnimatorConditionMode.IfNot, "IsCrouching"));

            // Walk -> Run: IsMoving=true, IsRunning=true, IsCrouching=false
            AddTransition(walkState, runState,
                (AnimatorConditionMode.If, "IsMoving"),
                (AnimatorConditionMode.If, "IsRunning"),
                (AnimatorConditionMode.IfNot, "IsCrouching"));

            // Walk -> Crouch_Walk: IsCrouching=true, IsMoving=true
            AddTransition(walkState, crouchWalkState,
                (AnimatorConditionMode.If, "IsCrouching"),
                (AnimatorConditionMode.If, "IsMoving"));

            // Walk -> Crouch_Idle: IsCrouching=true, IsMoving=false
            AddTransition(walkState, crouchIdleState,
                (AnimatorConditionMode.If, "IsCrouching"),
                (AnimatorConditionMode.IfNot, "IsMoving"));

            // --- Transitions from Run ---
            // Run -> Idle: IsMoving=false, IsCrouching=false
            AddTransition(runState, idleState,
                (AnimatorConditionMode.IfNot, "IsMoving"),
                (AnimatorConditionMode.IfNot, "IsCrouching"));

            // Run -> Walk: IsMoving=true, IsRunning=false, IsCrouching=false
            AddTransition(runState, walkState,
                (AnimatorConditionMode.If, "IsMoving"),
                (AnimatorConditionMode.IfNot, "IsRunning"),
                (AnimatorConditionMode.IfNot, "IsCrouching"));

            // Run -> Crouch_Walk: IsCrouching=true, IsMoving=true
            AddTransition(runState, crouchWalkState,
                (AnimatorConditionMode.If, "IsCrouching"),
                (AnimatorConditionMode.If, "IsMoving"));

            // --- Transitions from Crouch_Idle ---
            // Crouch_Idle -> Idle: IsCrouching=false, IsMoving=false
            AddTransition(crouchIdleState, idleState,
                (AnimatorConditionMode.IfNot, "IsCrouching"),
                (AnimatorConditionMode.IfNot, "IsMoving"));

            // Crouch_Idle -> Walk: IsCrouching=false, IsMoving=true, IsRunning=false
            AddTransition(crouchIdleState, walkState,
                (AnimatorConditionMode.IfNot, "IsCrouching"),
                (AnimatorConditionMode.If, "IsMoving"),
                (AnimatorConditionMode.IfNot, "IsRunning"));

            // Crouch_Idle -> Run: IsCrouching=false, IsMoving=true, IsRunning=true
            AddTransition(crouchIdleState, runState,
                (AnimatorConditionMode.IfNot, "IsCrouching"),
                (AnimatorConditionMode.If, "IsMoving"),
                (AnimatorConditionMode.If, "IsRunning"));

            // Crouch_Idle -> Crouch_Walk: IsCrouching=true, IsMoving=true
            AddTransition(crouchIdleState, crouchWalkState,
                (AnimatorConditionMode.If, "IsCrouching"),
                (AnimatorConditionMode.If, "IsMoving"));

            // --- Transitions from Crouch_Walk ---
            // Crouch_Walk -> Crouch_Idle: IsCrouching=true, IsMoving=false
            AddTransition(crouchWalkState, crouchIdleState,
                (AnimatorConditionMode.If, "IsCrouching"),
                (AnimatorConditionMode.IfNot, "IsMoving"));

            // Crouch_Walk -> Walk: IsCrouching=false, IsMoving=true, IsRunning=false
            AddTransition(crouchWalkState, walkState,
                (AnimatorConditionMode.IfNot, "IsCrouching"),
                (AnimatorConditionMode.If, "IsMoving"),
                (AnimatorConditionMode.IfNot, "IsRunning"));

            // Crouch_Walk -> Run: IsCrouching=false, IsMoving=true, IsRunning=true
            AddTransition(crouchWalkState, runState,
                (AnimatorConditionMode.IfNot, "IsCrouching"),
                (AnimatorConditionMode.If, "IsMoving"),
                (AnimatorConditionMode.If, "IsRunning"));

            // Crouch_Walk -> Idle: IsCrouching=false, IsMoving=false
            AddTransition(crouchWalkState, idleState,
                (AnimatorConditionMode.IfNot, "IsCrouching"),
                (AnimatorConditionMode.IfNot, "IsMoving"));

            EditorUtility.SetDirty(controller);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static AnimationClip CreateClip(Sprite[] sprites, float frameRate, string savePath)
        {
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(savePath);
            if (clip == null)
            {
                clip = new AnimationClip();
                AssetDatabase.CreateAsset(clip, savePath);
            }
            else
            {
                clip.ClearCurves();
            }

            clip.frameRate = frameRate;
            var binding = new EditorCurveBinding
            {
                type = typeof(SpriteRenderer),
                path = "",
                propertyName = "m_Sprite"
            };

            var keyframes = new ObjectReferenceKeyframe[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                keyframes[i] = new ObjectReferenceKeyframe
                {
                    time = i / frameRate,
                    value = sprites[i]
                };
            }

            AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);

            EditorUtility.SetDirty(clip);
            return clip;
        }

        private static void EnsureParameter(AnimatorController controller, string name, AnimatorControllerParameterType type)
        {
            if (!controller.parameters.Any(p => p.name == name))
            {
                controller.AddParameter(name, type);
            }
        }

        private static void AddTransition(AnimatorState from, AnimatorState to, params (AnimatorConditionMode mode, string param)[] conditions)
        {
            var trans = from.AddTransition(to);
            trans.hasExitTime = false;
            trans.duration = 0f;
            foreach (var (mode, param) in conditions)
            {
                trans.AddCondition(mode, 0f, param);
            }
        }
    }
}

