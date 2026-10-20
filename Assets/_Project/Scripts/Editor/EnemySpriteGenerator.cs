#nullable enable

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Aurora.Editor
{
    public static class EnemySpriteGenerator
    {
        private static readonly Color32 ColTrans = new Color32(0, 0, 0, 0);
        private static readonly Color32 ColBlack = new Color32(0, 0, 0, 255);
        private static readonly Color32 ColDark = new Color32(0x38, 0x28, 0x43, 255);
        private static readonly Color32 ColMauve = new Color32(0x7C, 0x6D, 0x80, 255);
        private static readonly Color32 ColLight = new Color32(0xC7, 0xC6, 0xC6, 255);

        public const int FrameWidth = 32;
        public const int FrameHeight = 48;

        [MenuItem("Aurora/Generate All Enemy Sprites & Animations")]
        public static void GenerateAllEnemies()
        {
            GenerateZombie();
            GenerateEyeGuy();
            GenerateGlitcher();
            GenerateBoss();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[Aurora] Successfully generated all 4 enemies (Sprites, Clips, Controllers)!");
        }

        public static void GenerateZombie()
        {
            string[] frameNames = new string[]
            {
                "Zombie_Idle_0", "Zombie_Idle_1",
                "Zombie_Move_0", "Zombie_Move_1", "Zombie_Move_2", "Zombie_Move_3", "Zombie_Move_4", "Zombie_Move_5"
            };

            string[][] frames = new string[][]
            {
                EnemyFrameData.ZombieIdle0, EnemyFrameData.ZombieIdle1,
                EnemyFrameData.ZombieMove0, EnemyFrameData.ZombieMove1, EnemyFrameData.ZombieMove2,
                EnemyFrameData.ZombieMove3, EnemyFrameData.ZombieMove4, EnemyFrameData.ZombieMove5
            };

            BuildEnemy("Zombie", "Assets/Assets/Sprites/Zombie_Sheet.png", frameNames, frames, 8f);
        }

        public static void GenerateEyeGuy()
        {
            string[] frameNames = new string[]
            {
                "EyeGuy_Idle_0", "EyeGuy_Idle_1",
                "EyeGuy_Move_0", "EyeGuy_Move_1", "EyeGuy_Move_2", "EyeGuy_Move_3", "EyeGuy_Move_4", "EyeGuy_Move_5"
            };

            string[][] frames = new string[][]
            {
                EnemyFrameData.EyeGuyIdle0, EnemyFrameData.EyeGuyIdle1,
                EnemyFrameData.EyeGuyMove0, EnemyFrameData.EyeGuyMove1, EnemyFrameData.EyeGuyMove2,
                EnemyFrameData.EyeGuyMove3, EnemyFrameData.EyeGuyMove4, EnemyFrameData.EyeGuyMove5
            };

            BuildEnemy("EyeGuy", "Assets/Assets/Sprites/EyeGuy_Sheet.png", frameNames, frames, 6f);
        }

        public static void GenerateGlitcher()
        {
            string[] frameNames = new string[]
            {
                "Glitcher_Idle_0", "Glitcher_Idle_1",
                "Glitcher_Move_0", "Glitcher_Move_1", "Glitcher_Move_2", "Glitcher_Move_3", "Glitcher_Move_4", "Glitcher_Move_5"
            };

            string[][] frames = new string[][]
            {
                EnemyFrameData.GlitcherIdle0, EnemyFrameData.GlitcherIdle1,
                EnemyFrameData.GlitcherMove0, EnemyFrameData.GlitcherMove1, EnemyFrameData.GlitcherMove2,
                EnemyFrameData.GlitcherMove3, EnemyFrameData.GlitcherMove4, EnemyFrameData.GlitcherMove5
            };

            BuildEnemy("Glitcher", "Assets/Assets/Sprites/Glitcher_Sheet.png", frameNames, frames, 10f);
        }

        public static void GenerateBoss()
        {
            string[] frameNames = new string[]
            {
                "Boss_Idle_0", "Boss_Idle_1",
                "Boss_Move_0", "Boss_Move_1", "Boss_Move_2", "Boss_Move_3", "Boss_Move_4", "Boss_Move_5"
            };

            string[][] frames = new string[][]
            {
                EnemyFrameData.BossIdle0, EnemyFrameData.BossIdle1,
                EnemyFrameData.BossMove0, EnemyFrameData.BossMove1, EnemyFrameData.BossMove2,
                EnemyFrameData.BossMove3, EnemyFrameData.BossMove4, EnemyFrameData.BossMove5
            };

            BuildEnemy("Boss", "Assets/Assets/Sprites/Boss_Sheet.png", frameNames, frames, 6f);
        }

        private static void BuildEnemy(string enemyName, string assetPath, string[] frameNames, string[][] frames, float moveFps)
        {
            int totalFrames = frameNames.Length;
            int texW = FrameWidth * totalFrames;
            int texH = FrameHeight;

            Texture2D tex = new Texture2D(texW, texH, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Point;

            Color32[] clearPixels = new Color32[texW * texH];
            for (int i = 0; i < clearPixels.Length; i++) clearPixels[i] = ColTrans;
            tex.SetPixels32(clearPixels);

            for (int f = 0; f < totalFrames; f++)
            {
                string[] lines = frames[f];
                int startX = f * FrameWidth;
                for (int row = 0; row < FrameHeight; row++)
                {
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

            string? dir = Path.GetDirectoryName(assetPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            byte[] pngBytes = tex.EncodeToPNG();
            File.WriteAllBytes(assetPath, pngBytes);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
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

            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
            dataProvider.InitSpriteEditorDataProvider();

            var spriteRects = new SpriteRect[totalFrames];
            for (int i = 0; i < totalFrames; i++)
            {
                spriteRects[i] = new SpriteRect
                {
                    name = frameNames[i],
                    rect = new Rect(i * FrameWidth, 0, FrameWidth, FrameHeight),
                    alignment = SpriteAlignment.Custom,
                    pivot = new Vector2(0.5f, 4f / FrameHeight), // Consistent ground baseline Y=4
                    border = Vector4.zero,
                    spriteID = GUID.Generate()
                };
            }

            dataProvider.SetSpriteRects(spriteRects);
            dataProvider.Apply();
            importer?.SaveAndReimport();

            // Create Animations & Animator Controller
            string animDir = $"Assets/_Project/Animations/{enemyName}";
            if (!Directory.Exists(animDir)) Directory.CreateDirectory(animDir);

            UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            var spriteMap = subAssets.OfType<Sprite>().ToDictionary(s => s.name, s => s);

            // Idle Clip (2 frames, 3 FPS)
            Sprite[] idleSprites = new Sprite[]
            {
                spriteMap[$"{enemyName}_Idle_0"],
                spriteMap[$"{enemyName}_Idle_1"],
                spriteMap[$"{enemyName}_Idle_0"]
            };
            AnimationClip idleClip = CreateClip(idleSprites, 3f, $"{animDir}/{enemyName}_Idle.anim");

            // Move Clip (6 frames, moveFps)
            Sprite[] moveSprites = new Sprite[totalFrames - 2 + 1];
            for (int i = 0; i < totalFrames - 2; i++)
            {
                moveSprites[i] = spriteMap[$"{enemyName}_Move_{i}"];
            }
            moveSprites[moveSprites.Length - 1] = moveSprites[0];
            AnimationClip moveClip = CreateClip(moveSprites, moveFps, $"{animDir}/{enemyName}_Move.anim");

            // Controller
            string controllerPath = $"{animDir}/{enemyName}_Controller.controller";
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if (controller == null)
            {
                controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            }

            if (!controller.parameters.Any(p => p.name == "IsMoving"))
            {
                controller.AddParameter("IsMoving", AnimatorControllerParameterType.Bool);
            }

            var rootSm = controller.layers[0].stateMachine;
            var states = rootSm.states.ToList();
            foreach (var st in states)
            {
                rootSm.RemoveState(st.state);
            }

            var idleState = rootSm.AddState("Idle");
            idleState.motion = idleClip;

            var moveState = rootSm.AddState("Move");
            moveState.motion = moveClip;

            rootSm.defaultState = idleState;

            // Idle -> Move (IsMoving = true)
            var transToMove = idleState.AddTransition(moveState);
            transToMove.hasExitTime = false;
            transToMove.duration = 0f;
            transToMove.AddCondition(AnimatorConditionMode.If, 0f, "IsMoving");

            // Move -> Idle (IsMoving = false)
            var transToIdle = moveState.AddTransition(idleState);
            transToIdle.hasExitTime = false;
            transToIdle.duration = 0f;
            transToIdle.AddCondition(AnimatorConditionMode.IfNot, 0f, "IsMoving");

            EditorUtility.SetDirty(controller);
            AssetDatabase.SaveAssets();
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
    }
}
