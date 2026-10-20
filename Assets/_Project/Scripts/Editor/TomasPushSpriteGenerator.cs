#nullable enable

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Aurora.Editor
{
    public static class TomasPushSpriteGenerator
    {
        // Palette (identical to Haunted Hotel Asset Pack / Tomas.png):
        // ' ' = Transparent (0,0,0,0)
        // '#' = Black #000000 (1px outline, soles, details)
        // 'D' = Dark Violet #382843 (Dark clothing/hair shadow, pants)
        // 'M' = Mid Mauve #7C6D80 (Hoodie, backpack, midtone dither)
        // '.' = Pale Silver #C7C6C6 (Skin/hands, shoe highlights, zipper/straps)

        private static readonly Color32 ColTrans = new Color32(0, 0, 0, 0);
        private static readonly Color32 ColBlack = new Color32(0, 0, 0, 255);
        private static readonly Color32 ColDark = new Color32(0x38, 0x28, 0x43, 255);
        private static readonly Color32 ColMauve = new Color32(0x7C, 0x6D, 0x80, 255);
        private static readonly Color32 ColLight = new Color32(0xC7, 0xC6, 0xC6, 255);

        public const int FrameWidth = 32;
        public const int FrameHeight = 48;
        public const int TotalFrames = 4;
        public const string AssetPath = "Assets/Assets/Sprites/Tomas_Push.png";
        public const string AnimDir = "Assets/_Project/Animations/Tomas";

        public static readonly string[] FrameNames = new string[]
        {
            "Tomas_Push_0",
            "Tomas_Push_1",
            "Tomas_Push_2",
            "Tomas_Push_3"
        };

        // 48 rows of 32 columns per frame.
        // Row 0 is top (y=47), Row 47 is bottom (y=0).
        // Ground baseline: Row 43 (y=4). Rows 44-47 are bottom padding.
        // Columns 0-1 and 30-31 are transparent padding on every frame.

        // Push 0: Contact / Drive 1 (Left leg forward bent, right leg braced back, hands pressing box)
        public static readonly string[] Push0 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "               ######           ", // 5 (y=42) Hair top leaning forward
            "             ##DDDDDD##         ", // 6 (y=41) Hair
            "            #DDMMDDDDMD#        ", // 7 (y=40)
            "           #DDMMDDDDMDDD#       ", // 8 (y=39)
            "          #DMDDDDDMMDDDD##      ", // 9 (y=38) Hair bangs
            "          #DDDD#DDDDDD#..#      ", // 10 (y=37) Forehead / Brow looking forward
            "          #DMD#.......#..#      ", // 11 (y=36) Eyes focused ahead
            "          #DD#.........##       ", // 12 (y=35) Nose / Cheek
            "           ##..#...####         ", // 13 (y=34) Clenched jaw / mouth
            "            #..M####            ", // 14 (y=33) Neck
            "         ####.M#                ", // 15 (y=32) Shoulder
            "       ##MM##M######            ", // 16 (y=31) Backpack top & Chest
            "      #MMMM##MMMMMM##           ", // 17 (y=30) Backpack & Hoodie
            "     #MMDMM#.#MMMMM..#          ", // 18 (y=29) Backpack strap
            "     #MDDMM#.#MMMMMDM#####      ", // 19 (y=28) Backpack & Upper arm thrusting forward
            "     #MMMMD#.#MMDMMMMMMMDM##    ", // 20 (y=27) Forearms pushing forward
            "     #DMMMD#.#DMMMMMMM#MDMM.#   ", // 21 (y=26) Arms extending, hands near box
            "     #DDMDD#.#MMMDMM#.##..#..#  ", // 22 (y=25) Hands pressing against box (col 27-29)
            "      #MDDD#.##DMMMD#..#..#..#  ", // 23 (y=24) Flat palms bracing
            "       #####..#DDDD##..#..###   ", // 24 (y=23) Hands against box
            "           #..#DDDDD####        ", // 25 (y=22) Waist / Belt
            "           #..#DDDDD#           ", // 26 (y=21) Pelvis
            "          #DD##DDDDD##          ", // 27 (y=20) Stride split
            "         #DD#   #DDDDD#         ", // 28 (y=19) Back thigh (L) / Front thigh (R)
            "        #DD#     #DDDDD#        ", // 29 (y=18)
            "       #DM#       #DDMDD#       ", // 30 (y=17)
            "      #DD#         #DMDDD#      ", // 31 (y=16) Front knee bent forward
            "     #DD#          #DDDDD#      ", // 32 (y=15)
            "    #DM#           #DDMDD#      ", // 33 (y=14)
            "   #DD#             #DDDDD#     ", // 34 (y=13) Front shin down
            "   #DD#             #DMDDD#     ", // 35 (y=12)
            "   #DM#             #DDDDD#     ", // 36 (y=11)
            "   #DD#             #DDMDD#     ", // 37 (y=10)
            "   #DD#             #DDDDD#     ", // 38 (y=9)
            "   #DD#             #DDDDD#     ", // 39 (y=8)
            "   ##..#           ##...##      ", // 40 (y=7)  Sneaker tongues
            "  #D...#          #D....#       ", // 41 (y=6)  Sneaker uppers
            "  #DDMM#          #DDMM.#       ", // 42 (y=5)  Sneaker mids
            "  ######          #######       ", // 43 (y=4)  GROUND LINE (both feet planted)
            "                                ", // 44 (y=3)
            "                                ", // 45 (y=2)
            "                                ", // 46 (y=1)
            "                                "  // 47 (y=0)
        };

        // Push 1: Step / Pass 1 (Left leg bearing weight, right leg lifting & advancing, torso dips 1px)
        public static readonly string[] Push1 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5 Body dips 1px with exertion
            "               ######           ", // 6 (y=41)
            "             ##DDDDDD##         ", // 7 (y=40)
            "            #DDMMDDDDMD#        ", // 8 (y=39)
            "           #DDMMDDDDMDDD#       ", // 9 (y=38)
            "          #DMDDDDDMMDDDD##      ", // 10 (y=37)
            "          #DDDD#DDDDDD#..#      ", // 11 (y=36)
            "          #DMD#.......#..#      ", // 12 (y=35)
            "          #DD#.........##       ", // 13 (y=34)
            "           ##..#...####         ", // 14 (y=33)
            "            #..M####            ", // 15 (y=32)
            "         ####.M#                ", // 16 (y=31)
            "       ##MM##M######            ", // 17 (y=30)
            "      #MMMM##MMMMMM##           ", // 18 (y=29)
            "     #MMDMM#.#MMMMM..#          ", // 19 (y=28)
            "     #MDDMM#.#MMMMMDM#####      ", // 20 (y=27)
            "     #MMMMD#.#MMDMMMMMMMDM##    ", // 21 (y=26)
            "     #DMMMD#.#DMMMMMMM#MDMM.#   ", // 22 (y=25)
            "     #DDMDD#.#MMMDMM#.##..#..#  ", // 23 (y=24)
            "      #MDDD#.##DMMMD#..#..#..#  ", // 24 (y=23)
            "       #####..#DDDD##..#..###   ", // 25 (y=22)
            "           #..#DDDDD####        ", // 26 (y=21)
            "           #..#DDDDD#           ", // 27 (y=20)
            "          #DD##DDDDD#           ", // 28 (y=19)
            "         #DD#  #DDDDD#          ", // 29 (y=18)
            "        #DD#    #DDMDD#         ", // 30 (y=17)
            "       #DM#      #DMDDD#        ", // 31 (y=16)
            "      ##..#      #DDDDD#        ", // 32 (y=15) Rear foot lifted & bent
            "     #D...#      #DDMDD#        ", // 33 (y=14)
            "     #DDMM#       #DDDDD#       ", // 34 (y=13)
            "     ######       #DMDDD#       ", // 35 (y=12) Lifted foot sole
            "                  #DDDDD#       ", // 36 (y=11)
            "                  #DDMDD#       ", // 37 (y=10)
            "                  #DDDDD#       ", // 38 (y=9)
            "                  #DDDDD#       ", // 39 (y=8)
            "                 ##...##        ", // 40 (y=7)  Planted front sneaker tongue
            "                #D....#         ", // 41 (y=6)  Sneaker upper
            "                #DDMM.#         ", // 42 (y=5)  Sneaker mid
            "                #######         ", // 43 (y=4)  GROUND LINE (front foot planted)
            "                                ", // 44 (y=3)
            "                                ", // 45 (y=2)
            "                                ", // 46 (y=1)
            "                                "  // 47 (y=0)
        };

        // Push 2: Contact / Drive 2 (Right leg forward bent, left leg braced back, hands pressing box)
        public static readonly string[] Push2 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "               ######           ", // 5 (y=42)
            "             ##DDDDDD##         ", // 6 (y=41)
            "            #DDMMDDDDMD#        ", // 7 (y=40)
            "           #DDMMDDDDMDDD#       ", // 8 (y=39)
            "          #DMDDDDDMMDDDD##      ", // 9 (y=38)
            "          #DDDD#DDDDDD#..#      ", // 10 (y=37)
            "          #DMD#.......#..#      ", // 11 (y=36)
            "          #DD#.........##       ", // 12 (y=35)
            "           ##..#...####         ", // 13 (y=34)
            "            #..M####            ", // 14 (y=33)
            "         ####.M#                ", // 15 (y=32)
            "       ##MM##M######            ", // 16 (y=31)
            "      #MMMM##MMMMMM##           ", // 17 (y=30)
            "     #MMDMM#.#MMMMM..#          ", // 18 (y=29)
            "     #MDDMM#.#MMMMMDM#####      ", // 19 (y=28)
            "     #MMMMD#.#MMDMMMMMMMDM##    ", // 20 (y=27)
            "     #DMMMD#.#DMMMMMMM#MDMM.#   ", // 21 (y=26)
            "     #DDMDD#.#MMMDMM#.##..#..#  ", // 22 (y=25)
            "      #MDDD#.##DMMMD#..#..#..#  ", // 23 (y=24)
            "       #####..#DDDD##..#..###   ", // 24 (y=23)
            "           #..#DDDDD####        ", // 25 (y=22)
            "           #..#DDDDD#           ", // 26 (y=21)
            "          #DD##DDDDD##          ", // 27 (y=20) Opposite stride split
            "         #DMD#  #DDDDD#         ", // 28 (y=19)
            "        #DDDD#   #DDDDD#        ", // 29 (y=18)
            "       #DDMDD#    #DDMDD#       ", // 30 (y=17)
            "      #DMDDD#      #DMDDD#      ", // 31 (y=16) Front knee bent forward
            "     #DDDDD#       #DDDDD#      ", // 32 (y=15)
            "    #DDMDD#        #DDMDD#      ", // 33 (y=14)
            "   #DDDDD#          #DDDDD#     ", // 34 (y=13) Front shin down
            "   #DMDDD#          #DMDDD#     ", // 35 (y=12)
            "   #DDDDD#          #DDDDD#     ", // 36 (y=11)
            "   #DDMDD#          #DDMDD#     ", // 37 (y=10)
            "   #DDDDD#          #DDDDD#     ", // 38 (y=9)
            "   #DDDDD#          #DDDDD#     ", // 39 (y=8)
            "   ##...##         ##..#        ", // 40 (y=7)  Sneaker tongues
            "  #D....#         #D...#        ", // 41 (y=6)  Sneaker uppers
            "  #DDMM.#         #DDMM#        ", // 42 (y=5)  Sneaker mids
            "  #######         ######        ", // 43 (y=4)  GROUND LINE (both feet planted)
            "                                ", // 44 (y=3)
            "                                ", // 45 (y=2)
            "                                ", // 46 (y=1)
            "                                "  // 47 (y=0)
        };

        // Push 3: Step / Pass 2 (Right leg bearing weight, left leg lifting & advancing, torso dips 1px)
        public static readonly string[] Push3 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5 Body dips 1px with exertion
            "               ######           ", // 6 (y=41)
            "             ##DDDDDD##         ", // 7 (y=40)
            "            #DDMMDDDDMD#        ", // 8 (y=39)
            "           #DDMMDDDDMDDD#       ", // 9 (y=38)
            "          #DMDDDDDMMDDDD##      ", // 10 (y=37)
            "          #DDDD#DDDDDD#..#      ", // 11 (y=36)
            "          #DMD#.......#..#      ", // 12 (y=35)
            "          #DD#.........##       ", // 13 (y=34)
            "           ##..#...####         ", // 14 (y=33)
            "            #..M####            ", // 15 (y=32)
            "         ####.M#                ", // 16 (y=31)
            "       ##MM##M######            ", // 17 (y=30)
            "      #MMMM##MMMMMM##           ", // 18 (y=29)
            "     #MMDMM#.#MMMMM..#          ", // 19 (y=28)
            "     #MDDMM#.#MMMMMDM#####      ", // 20 (y=27)
            "     #MMMMD#.#MMDMMMMMMMDM##    ", // 21 (y=26)
            "     #DMMMD#.#DMMMMMMM#MDMM.#   ", // 22 (y=25)
            "     #DDMDD#.#MMMDMM#.##..#..#  ", // 23 (y=24)
            "      #MDDD#.##DMMMD#..#..#..#  ", // 24 (y=23)
            "       #####..#DDDD##..#..###   ", // 25 (y=22)
            "           #..#DDDDD####        ", // 26 (y=21)
            "           #..#DDDDD#           ", // 27 (y=20)
            "          #DD##DDDDD#           ", // 28 (y=19)
            "         #DMDD# #DDDD#          ", // 29 (y=18)
            "        #DDDDD#  #DDMDD#        ", // 30 (y=17)
            "        #DDMDD#   #DMDDD#       ", // 31 (y=16)
            "       ##...##    #DDDDD#       ", // 32 (y=15) Opposite foot lifted & bent
            "      #D....#     #DDMDD#       ", // 33 (y=14)
            "      #DDMM.#      #DDDDD#      ", // 34 (y=13)
            "      #######      #DMDDD#      ", // 35 (y=12) Lifted foot sole
            "                   #DDDDD#      ", // 36 (y=11)
            "                   #DDMDD#      ", // 37 (y=10)
            "                   #DDDDD#      ", // 38 (y=9)
            "                   #DDDDD#      ", // 39 (y=8)
            "                  ##...##       ", // 40 (y=7)  Planted sneaker tongue
            "                 #D....#        ", // 41 (y=6)  Sneaker upper
            "                 #DDMM.#        ", // 42 (y=5)  Sneaker mid
            "                 #######        ", // 43 (y=4)  GROUND LINE (right foot planted)
            "                                ", // 44 (y=3)
            "                                ", // 45 (y=2)
            "                                ", // 46 (y=1)
            "                                "  // 47 (y=0)
        };

        [MenuItem("Aurora/Generate Tomas Push Sprites")]
        public static void GenerateSpritesMenuItem()
        {
            Generate();
            SetupAnimations();
            Debug.Log($"[Aurora] Generated Tomas Push sprite sheet and animation at {AssetPath} & {AnimDir}/Tomas_Push.anim");
        }

        public static void Generate()
        {
            int texW = FrameWidth * TotalFrames; // 128
            int texH = FrameHeight; // 48

            Texture2D tex = new Texture2D(texW, texH, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Point;

            Color32[] clearPixels = new Color32[texW * texH];
            for (int i = 0; i < clearPixels.Length; i++) clearPixels[i] = ColTrans;
            tex.SetPixels32(clearPixels);

            string[][] frames = new string[][]
            {
                Push0,
                Push1,
                Push2,
                Push3
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

            // Configure TextureImporter
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
                importer.spritePixelsPerUnit = 100;
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
                    pivot = new Vector2(0.5f, 4f / FrameHeight), // Baseline at y=4
                    border = Vector4.zero,
                    spriteID = GUID.Generate()
                };
            }

            dataProvider.SetSpriteRects(spriteRects);
            dataProvider.Apply();
            importer?.SaveAndReimport();
        }

        public static void SetupAnimations()
        {
            if (!Directory.Exists(AnimDir)) Directory.CreateDirectory(AnimDir);

            Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetPath);
            var spriteMap = subAssets.OfType<Sprite>().ToDictionary(s => s.name, s => s);

            if (spriteMap.Count < TotalFrames)
            {
                Debug.LogWarning($"[Aurora] Expected {TotalFrames} sprites in {AssetPath}, found {spriteMap.Count}.");
                return;
            }

            // Push Clip (4 frames cycle at 5 FPS - heavy and deliberate movement)
            string clipPath = $"{AnimDir}/Tomas_Push.anim";
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            if (clip == null)
            {
                clip = new AnimationClip();
                AssetDatabase.CreateAsset(clip, clipPath);
            }
            else
            {
                clip.ClearCurves();
            }

            float frameRate = 5f;
            clip.frameRate = frameRate;
            var binding = new EditorCurveBinding
            {
                type = typeof(SpriteRenderer),
                path = "",
                propertyName = "m_Sprite"
            };

            var keyframes = new ObjectReferenceKeyframe[]
            {
                new ObjectReferenceKeyframe { time = 0f / frameRate, value = spriteMap["Tomas_Push_0"] },
                new ObjectReferenceKeyframe { time = 1f / frameRate, value = spriteMap["Tomas_Push_1"] },
                new ObjectReferenceKeyframe { time = 2f / frameRate, value = spriteMap["Tomas_Push_2"] },
                new ObjectReferenceKeyframe { time = 3f / frameRate, value = spriteMap["Tomas_Push_3"] },
                new ObjectReferenceKeyframe { time = 4f / frameRate, value = spriteMap["Tomas_Push_0"] }
            };

            AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);

            EditorUtility.SetDirty(clip);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
