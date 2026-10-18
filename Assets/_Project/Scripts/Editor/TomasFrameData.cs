#nullable enable

namespace Aurora.Editor
{
    public static class TomasFrameData
    {
        // 48 rows of 32 columns.
        // Row 0 is top (y=47), Row 47 is bottom (y=0).
        // Ground line: Row 43 (y=4). Rows 44-47 are bottom padding. Rows 0-5 are top padding.

        public static readonly string[] Idle0 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "             ######             ", // 5 (y=42)
            "           ##DDDDDD##           ", // 6 (y=41) Top hair
            "          #DDMMDDDDMD#          ", // 7 (y=40)
            "         #DDMMDDDDMDDD#         ", // 8 (y=39)
            "        #DMDDDDDMMDDDD##        ", // 9 (y=38) Hair bangs
            "        #DDDD#DDDDDD#..#        ", // 10 (y=37) Forehead / Brow
            "        #DMD#.......#..#        ", // 11 (y=36) Eyes (# at 18) / Nose tip
            "        #DD#.........##         ", // 12 (y=35) Cheek / Nose bridge
            "         ##...#...###           ", // 13 (y=34) Mouth & Chin
            "          #..M####              ", // 14 (y=33) Jaw / Neck
            "        ###.M#                  ", // 15 (y=32) Collar / Shoulder
            "      ##MM##M######             ", // 16 (y=31) Backpack top & Chest
            "     #MMMM##MMMMMM##            ", // 17 (y=30) Backpack & Hoodie chest
            "    #MMDMM#.#MMMMM..#           ", // 18 (y=29) Backpack & Strap (.)
            "    #MDDMM#.#MMMMMDM#           ", // 19 (y=28) Backpack & Zipper
            "    #MMMMD#.#MMDMMM##           ", // 20 (y=27) Backpack & Torso
            "    #DMMMD#.#DMMMMM#.#          ", // 21 (y=26) Arm sleeve start
            "    #DDMDD#.#MMMDMM#.##         ", // 22 (y=25) Arm sleeve
            "     #MDDD#.##DMMMD#..#         ", // 23 (y=24) Forearm
            "      #####..#DMMMD#..#         ", // 24 (y=23) Hand (.)
            "          #..#DDDDD####         ", // 25 (y=22) Waist / Belt
            "          #..#DDDDD#            ", // 26 (y=21) Pelvis
            "          #DD#DDDDD#            ", // 27 (y=20) Upper thighs
            "          #DD#DMDDD#            ", // 28 (y=19) Thighs
            "          #DD#DDDDD#            ", // 29 (y=18) Mid thighs
            "          #DM#DDMDD#            ", // 30 (y=17) Above knees
            "          #DD#DMDDD#            ", // 31 (y=16) Knees
            "          #DD#DDDDD#            ", // 32 (y=15) Calves
            "          #DM#DDMDD#            ", // 33 (y=14) Calves
            "          #DD#DDDDD#            ", // 34 (y=13) Lower calves
            "          #DD#DMDDD#            ", // 35 (y=12) Shins
            "          #DM#DDDDD#            ", // 36 (y=11) Shins
            "          #DD#DDMDD#            ", // 37 (y=10) Above ankles
            "          #DD#DDDDD#            ", // 38 (y=9)  Ankles
            "          #DD#DDDDD#            ", // 39 (y=8)  Cuffs
            "         ##..##...##            ", // 40 (y=7)  Sneaker tongues (.)
            "        #D....#D...#            ", // 41 (y=6)  Sneaker uppers
            "        #DDMM.#DDMM#            ", // 42 (y=5)  Sneaker mids
            "        #######.####            ", // 43 (y=4)  GROUND LINE (soles flat)
            "                                ", // 44 (y=3)
            "                                ", // 45 (y=2)
            "                                ", // 46 (y=1)
            "                                "  // 47 (y=0)
        };

        public static readonly string[] Idle1 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "             ######             ", // 4 (y=43) Subtle breathing lift
            "           ##DDDDDD##           ", // 5 (y=42)
            "          #DDMMDDDDMD#          ", // 6 (y=41)
            "         #DDMMDDDDMDDD#         ", // 7 (y=40)
            "        #DMDDDDDMMDDDD##        ", // 8 (y=39)
            "        #DDDD#DDDDDD#..#        ", // 9 (y=38)
            "        #DMD#.......#..#        ", // 10 (y=37)
            "        #DD#.........##         ", // 11 (y=36)
            "         ##...#...###           ", // 12 (y=35)
            "          #..M####              ", // 13 (y=34)
            "        ###.M#                  ", // 14 (y=33)
            "      ##MM##M######             ", // 15 (y=32)
            "     #MMMM##MMMMMM##            ", // 16 (y=31)
            "    #MMDMM#.#MMMMM..#           ", // 17 (y=30)
            "    #MDDMM#.#MMMMMDM#           ", // 18 (y=29)
            "    #MMMMD#.#MMDMMM##           ", // 19 (y=28)
            "    #DMMMD#.#DMMMMM#.#          ", // 20 (y=27)
            "    #DDMDD#.#MMMDMM#.##         ", // 21 (y=26)
            "     #MDDD#.##DMMMD#..#         ", // 22 (y=25)
            "      #####..#DMMMD#..#         ", // 23 (y=24)
            "          #..#DDDDD####         ", // 24 (y=23)
            "          #..#DDDDD#            ", // 25 (y=22)
            "          #DD#DDDDD#            ", // 26 (y=21)
            "          #DD#DMDDD#            ", // 27 (y=20)
            "          #DD#DDDDD#            ", // 28 (y=19)
            "          #DM#DDMDD#            ", // 29 (y=18)
            "          #DD#DMDDD#            ", // 30 (y=17)
            "          #DD#DDDDD#            ", // 31 (y=16)
            "          #DM#DDMDD#            ", // 32 (y=15)
            "          #DD#DDDDD#            ", // 33 (y=14)
            "          #DD#DMDDD#            ", // 34 (y=13)
            "          #DM#DDDDD#            ", // 35 (y=12)
            "          #DD#DDMDD#            ", // 36 (y=11)
            "          #DD#DDDDD#            ", // 37 (y=10)
            "          #DD#DDDDD#            ", // 38 (y=9)
            "          #DD#DDDDD#            ", // 39 (y=8)
            "         ##..##...##            ", // 40 (y=7)
            "        #D....#D...#            ", // 41 (y=6)
            "        #DDMM.#DDMM#            ", // 42 (y=5)
            "        #######.####            ", // 43 (y=4)  GROUND LINE (identical foot base)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Walk 0: Contact 1 (Right leg forward, Left leg back)
        public static readonly string[] Walk0 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "             ######             ", // 5
            "           ##DDDDDD##           ", // 6
            "          #DDMMDDDDMD#          ", // 7
            "         #DDMMDDDDMDDD#         ", // 8
            "        #DMDDDDDMMDDDD##        ", // 9
            "        #DDDD#DDDDDD#..#        ", // 10
            "        #DMD#.......#..#        ", // 11
            "        #DD#.........##         ", // 12
            "         ##...#...###           ", // 13
            "          #..M####              ", // 14
            "        ###.M#                  ", // 15
            "      ##MM##M######             ", // 16
            "     #MMMM##MMMMMM##            ", // 17
            "    #MMDMM#.#MMMMM..#           ", // 18
            "    #MDDMM#.#MMMMMDM#           ", // 19
            "    #MMMMD#.#MMDMMM##           ", // 20
            "    #DMMMD#.#DMMMMM#.#          ", // 21
            "    #DDMDD#.#MMMDMM#.##         ", // 22
            "     #MDDD#.##DMMMD#..#         ", // 23
            "      #####..#DMMMD#..#         ", // 24
            "          #..#DDDDD####         ", // 25
            "          #..#DDDDD#            ", // 26
            "         #DD##DDDDD##           ", // 27 Leg split begins
            "        #DD#  #DMDDD#           ", // 28
            "       #DD#    #DDDDD#          ", // 29
            "      #DM#      #DDMDD#         ", // 30
            "     #DD#        #DMDDD#        ", // 31
            "    #DD#          #DDDDD#       ", // 32
            "   #DM#            #DDMDD#      ", // 33
            "  #DD#              #DDDDD#     ", // 34
            "  #DD#              #DMDDD#     ", // 35
            "  #DM#              #DDDDD#     ", // 36
            "  #DD#              #DDMDD#     ", // 37
            "  #DD#              #DDDDD#     ", // 38
            "  #DD#              #DDDDD#     ", // 39
            " ##..#             ##...##      ", // 40
            "#D...#            #D....#       ", // 41
            "#DDMM#            #DDMM.#       ", // 42
            "######            #######       ", // 43 GROUND LINE Y=4 (both feet touch)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Walk 1: Passing 1 (Right leg bearing weight, left leg bending and lifting)
        public static readonly string[] Walk1 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5 Body dips 1px
            "             ######             ", // 6
            "           ##DDDDDD##           ", // 7
            "          #DDMMDDDDMD#          ", // 8
            "         #DDMMDDDDMDDD#         ", // 9
            "        #DMDDDDDMMDDDD##        ", // 10
            "        #DDDD#DDDDDD#..#        ", // 11
            "        #DMD#.......#..#        ", // 12
            "        #DD#.........##         ", // 13
            "         ##...#...###           ", // 14
            "          #..M####              ", // 15
            "        ###.M#                  ", // 16
            "      ##MM##M######             ", // 17
            "     #MMMM##MMMMMM##            ", // 18
            "    #MMDMM#.#MMMMM..#           ", // 19
            "    #MDDMM#.#MMMMMDM#           ", // 20
            "    #MMMMD#.#MMDMMM##           ", // 21
            "    #DMMMD#.#DMMMMM#.#          ", // 22
            "    #DDMDD#.#MMMDMM#.##         ", // 23
            "     #MDDD#.##DMMMD#..#         ", // 24
            "      #####..#DMMMD#..#         ", // 25
            "          #..#DDDDD####         ", // 26
            "          #..#DDDDD#            ", // 27
            "          #DD#DDDDD#            ", // 28
            "         #DD# #DMDDD#           ", // 29
            "        #DD#   #DDDDD#          ", // 30 Left leg bent at knee
            "       #DM#    #DDMDD#          ", // 31
            "      #DD#     #DMDDD#          ", // 32
            "     ##..#     #DDDDD#          ", // 33 Left foot lifted
            "    #D...#     #DDMDD#          ", // 34
            "    #DDMM#     #DDDDD#          ", // 35
            "    ######     #DMDDD#          ", // 36 Left sole at Y=11
            "               #DDDDD#          ", // 37
            "               #DDMDD#          ", // 38
            "               #DDDDD#          ", // 39
            "              ##...##           ", // 40
            "             #D....#            ", // 41
            "             #DDMM.#            ", // 42
            "             #######            ", // 43 GROUND LINE Y=4 (right foot flat)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Walk 2: Push-off / Extension 1 (Right foot pushing off, left foot swinging forward)
        public static readonly string[] Walk2 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "             ######             ", // 4 Peak height
            "           ##DDDDDD##           ", // 5
            "          #DDMMDDDDMD#          ", // 6
            "         #DDMMDDDDMDDD#         ", // 7
            "        #DMDDDDDMMDDDD##        ", // 8
            "        #DDDD#DDDDDD#..#        ", // 9
            "        #DMD#.......#..#        ", // 10
            "        #DD#.........##         ", // 11
            "         ##...#...###           ", // 12
            "          #..M####              ", // 13
            "        ###.M#                  ", // 14
            "      ##MM##M######             ", // 15
            "     #MMMM##MMMMMM##            ", // 16
            "    #MMDMM#.#MMMMM..#           ", // 17
            "    #MDDMM#.#MMMMMDM#           ", // 18
            "    #MMMMD#.#MMDMMM##           ", // 19
            "    #DMMMD#.#DMMMMM#.#          ", // 20
            "    #DDMDD#.#MMMDMM#.##         ", // 21
            "     #MDDD#.##DMMMD#..#         ", // 22
            "      #####..#DMMMD#..#         ", // 23
            "          #..#DDDDD####         ", // 24
            "          #..#DDDDD#            ", // 25
            "          #DD##DDDD#            ", // 26
            "         #DD#  #DDDD#           ", // 27
            "        #DD#    #DDDD#          ", // 28
            "        #DM#     #DDDD#         ", // 29 Left leg swinging forward
            "        #DD#      #DDDD#        ", // 30
            "        #DD#       #DDDD#       ", // 31
            "        #DM#        #DDDD#      ", // 32
            "        #DD#        ##...##     ", // 33 Left foot extending
            "        #DD#       #D....#      ", // 34
            "        #DM#       #DDMM.#      ", // 35
            "        #DD#       #######      ", // 36 Left sole at Y=11
            "        #DD#                    ", // 37
            "        #DD#                    ", // 38
            "       ##..##                   ", // 39
            "      #D....#                   ", // 40
            "      #DDMM.#                   ", // 41
            "      #.#####                   ", // 42 Ball of right foot
            "      ###                       ", // 43 GROUND LINE Y=4 (push off point)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Walk 3: Contact 2 (Left leg forward, Right leg back)
        public static readonly string[] Walk3 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "             ######             ", // 5
            "           ##DDDDDD##           ", // 6
            "          #DDMMDDDDMD#          ", // 7
            "         #DDMMDDDDMDDD#         ", // 8
            "        #DMDDDDDMMDDDD##        ", // 9
            "        #DDDD#DDDDDD#..#        ", // 10
            "        #DMD#.......#..#        ", // 11
            "        #DD#.........##         ", // 12
            "         ##...#...###           ", // 13
            "          #..M####              ", // 14
            "        ###.M#                  ", // 15
            "      ##MM##M######             ", // 16
            "     #MMMM##MMMMMM##            ", // 17
            "    #MMDMM#.#MMMMM..#           ", // 18
            "    #MDDMM#.#MMMMMDM#           ", // 19
            "    #MMMMD#.#MMDMMM##           ", // 20
            "    #DMMMD#.#DMMMMM#.#          ", // 21
            "    #DDMDD#.#MMMDMM#.##         ", // 22
            "     #MDDD#.##DMMMD#..#         ", // 23
            "      #####..#DMMMD#..#         ", // 24
            "          #..#DDDDD####         ", // 25
            "          #..#DDDDD#            ", // 26
            "         #DD##DDDDD##           ", // 27 Opposite leg split
            "        #DMD# #DDDDD#           ", // 28
            "       #DDDD#  #DDDDD#          ", // 29
            "      #DDMDD#   #DDMDD#         ", // 30
            "     #DMDDD#     #DMDDD#        ", // 31
            "    #DDDDD#       #DDDDD#       ", // 32
            "   #DDMDD#         #DDMDD#      ", // 33
            "  #DDDDD#           #DDDDD#     ", // 34
            "  #DMDDD#           #DMDDD#     ", // 35
            "  #DDDDD#           #DDDDD#     ", // 36
            "  #DDMDD#           #DDMDD#     ", // 37
            "  #DDDDD#           #DDDDD#     ", // 38
            "  #DDDDD#           #DDDDD#     ", // 39
            " ##...##           ##..#        ", // 40
            "#D....#           #D...#        ", // 41
            "#DDMM.#           #DDMM#        ", // 42
            "#######           ######        ", // 43 GROUND LINE Y=4 (both feet touch)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Walk 4: Passing 2 (Left leg bearing weight, right leg bending and lifting)
        public static readonly string[] Walk4 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5 Body dips 1px
            "             ######             ", // 6
            "           ##DDDDDD##           ", // 7
            "          #DDMMDDDDMD#          ", // 8
            "         #DDMMDDDDMDDD#         ", // 9
            "        #DMDDDDDMMDDDD##        ", // 10
            "        #DDDD#DDDDDD#..#        ", // 11
            "        #DMD#.......#..#        ", // 12
            "        #DD#.........##         ", // 13
            "         ##...#...###           ", // 14
            "          #..M####              ", // 15
            "        ###.M#                  ", // 16
            "      ##MM##M######             ", // 17
            "     #MMMM##MMMMMM##            ", // 18
            "    #MMDMM#.#MMMMM..#           ", // 19
            "    #MDDMM#.#MMMMMDM#           ", // 20
            "    #MMMMD#.#MMDMMM##           ", // 21
            "    #DMMMD#.#DMMMMM#.#          ", // 22
            "    #DDMDD#.#MMMDMM#.##         ", // 23
            "     #MDDD#.##DMMMD#..#         ", // 24
            "      #####..#DMMMD#..#         ", // 25
            "          #..#DDDDD####         ", // 26
            "          #..#DDDDD#            ", // 27
            "          #DD#DDDDD#            ", // 28
            "         #DMDD# #DD#            ", // 29
            "        #DDDDD#  #DD#           ", // 30 Right leg bent
            "        #DDMDD#   #DM#          ", // 31
            "        #DMDDD#    #DD#         ", // 32
            "        #DDDDD#    ##..#        ", // 33 Right foot lifted
            "        #DDMDD#    #D...#       ", // 34
            "        #DDDDD#    #DDMM#       ", // 35
            "        #DMDDD#    ######       ", // 36 Right sole at Y=11
            "        #DDDDD#                 ", // 37
            "        #DDMDD#                 ", // 38
            "        #DDDDD#                 ", // 39
            "       ##...##                  ", // 40
            "      #D....#                   ", // 41
            "      #DDMM.#                   ", // 42
            "      #######                   ", // 43 GROUND LINE Y=4 (left foot flat)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Walk 5: Push-off / Extension 2 (Left foot pushing off, right leg extending forward)
        public static readonly string[] Walk5 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "             ######             ", // 4 Peak height
            "           ##DDDDDD##           ", // 5
            "          #DDMMDDDDMD#          ", // 6
            "         #DDMMDDDDMDDD#         ", // 7
            "        #DMDDDDDMMDDDD##        ", // 8
            "        #DDDD#DDDDDD#..#        ", // 9
            "        #DMD#.......#..#        ", // 10
            "        #DD#.........##         ", // 11
            "         ##...#...###           ", // 12
            "          #..M####              ", // 13
            "        ###.M#                  ", // 14
            "      ##MM##M######             ", // 15
            "     #MMMM##MMMMMM##            ", // 16
            "    #MMDMM#.#MMMMM..#           ", // 17
            "    #MDDMM#.#MMMMMDM#           ", // 18
            "    #MMMMD#.#MMDMMM##           ", // 19
            "    #DMMMD#.#DMMMMM#.#          ", // 20
            "    #DDMDD#.#MMMDMM#.##         ", // 21
            "     #MDDD#.##DMMMD#..#         ", // 22
            "      #####..#DMMMD#..#         ", // 23
            "          #..#DDDDD####         ", // 24
            "          #..#DDDDD#            ", // 25
            "          #DD##DDDD#            ", // 26
            "          #DDDD# #DD#           ", // 27
            "          #DDDD#  #DD#          ", // 28
            "         #DDDD#   #DM#          ", // 29
            "        #DDDD#    #DD#          ", // 30
            "       #DDDD#     #DD#          ", // 31
            "      #DDDD#      #DM#          ", // 32
            "     ##...##      #DD#          ", // 33 Right foot extending
            "    #D....#       #DD#          ", // 34
            "    #DDMM.#       #DM#          ", // 35
            "    #######       #DD#          ", // 36
            "                  #DD#          ", // 37
            "                  #DD#          ", // 38
            "                 ##..##         ", // 39
            "                #D....#         ", // 40
            "                #DDMM.#         ", // 41
            "                #.#####         ", // 42 Ball of left foot
            "                ###             ", // 43 GROUND LINE Y=4 (push off point)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // ==========================================
        // RUN CYCLE (6 frames, dynamic forward lean, wide stride)
        // ==========================================

        // Run 0: Right foot heel strike forward, left leg high trail, arms forward/back
        public static readonly string[] Run0 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "               ######           ", // 5 Leaning forward
            "             ##DDDDDD##         ", // 6
            "            #DDMMDDDDMD#        ", // 7
            "           #DDMMDDDDMDDD#       ", // 8
            "          #DMDDDDDMMDDDD##      ", // 9 Hair swept back
            "          #DDDD#DDDDDD#..#      ", // 10
            "          #DMD#.......#..#      ", // 11
            "          #DD#.........##       ", // 12
            "           ##...#...###         ", // 13
            "            #..M####            ", // 14
            "         ####.M#                ", // 15
            "       ##MM##M######            ", // 16 Forward tilted torso
            "      #MMMM##MMMMMM##           ", // 17
            "     #MMDMM#.#MMMMM..#          ", // 18 Left arm pumping forward
            "     #MDDMM#.#MMMMMDM#          ", // 19
            "     #MMMMD#.#MMDMMM##..#       ", // 20 Hand forward (.)
            "     #DMMMD#.#DMMMMM#..#        ", // 21
            "     #DDMDD#.#MMMDMM#.##        ", // 22
            "      #MDDD#.##DMMMD#           ", // 23
            "   #   #####..#DDDDD####        ", // 24 Right arm back
            "  #.#      #..#DDDDD#           ", // 25
            "   #       #DD#DDDDD#           ", // 26 Split stride
            "          #DD# #DDDDD#          ", // 27
            "         #DD#   #DDDDD#         ", // 28
            "        #DD#     #DDMDD#        ", // 29
            "       #DM#       #DMDDD#       ", // 30
            "      #DD#         #DDDDD#      ", // 31
            "  ### #DD#          #DDMDD#     ", // 32
            " #D..##DD#           #DDDDD#    ", // 33 Right foot reaching forward
            " #DDM##DM#            #DMDDD#   ", // 34
            " ##### #D#             #DDDDD#  ", // 35 Left foot high in back (Y=12)
            "                       #DDMDD#  ", // 36
            "                       #DDDDD#  ", // 37
            "                       #DDDDD#  ", // 38
            "                      ##...##   ", // 39
            "                     #D....#    ", // 40
            "                     #DDMM.#    ", // 41
            "                     #######    ", // 42 Right heel strike at ground
            "                     #######    ", // 43 GROUND LINE Y=4 (heel contact)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Run 1: Right leg compression / weight bearing, body dips 1px, left knee drives
        public static readonly string[] Run1 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5 Body dips 1px
            "               ######           ", // 6
            "             ##DDDDDD##         ", // 7
            "            #DDMMDDDDMD#        ", // 8
            "           #DDMMDDDDMDDD#       ", // 9
            "          #DMDDDDDMMDDDD##      ", // 10
            "          #DDDD#DDDDDD#..#      ", // 11
            "          #DMD#.......#..#      ", // 12
            "          #DD#.........##       ", // 13
            "           ##...#...###         ", // 14
            "            #..M####            ", // 15
            "         ####.M#                ", // 16
            "       ##MM##M######            ", // 17
            "      #MMMM##MMMMMM##           ", // 18
            "     #MMDMM#.#MMMMM..#          ", // 19
            "     #MDDMM#.#MMMMMDM#          ", // 20
            "     #MMMMD#.#MMDMMM##          ", // 21
            "     #DMMMD#.#DMMMMM#.#         ", // 22
            "     #DDMDD#.#MMMDMM#.##        ", // 23
            "      #MDDD#.##DMMMD#..#        ", // 24
            "       #####..#DDDDD####        ", // 25
            "           #..#DDDDD#           ", // 26
            "           #DD#DDDDD#           ", // 27 Left leg bending
            "          #DD# #DDDDD#          ", // 28
            "         #DD#   #DDMDD#         ", // 29
            "        #DM#     #DMDDD#        ", // 30
            "       #DD#       #DDDDD#       ", // 31
            "      ##..#       #DDMDD#       ", // 32 Left foot passing
            "     #D...#       #DDDDD#       ", // 33
            "     #DDMM#       #DMDDD#       ", // 34
            "     ######       #DDDDD#       ", // 35 Left sole at Y=12
            "                  #DDMDD#       ", // 36
            "                  #DDDDD#       ", // 37
            "                  #DDDDD#       ", // 38
            "                 ##...##        ", // 39
            "                #D....#         ", // 40
            "                #DDMM.#         ", // 41
            "                #######         ", // 42 Right foot fully planted
            "                #######         ", // 43 GROUND LINE Y=4 (full plant)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Run 2: Right leg push-off / propulsion, apex height, left leg swings forward
        public static readonly string[] Run2 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "               ######           ", // 4 Apex rise
            "             ##DDDDDD##         ", // 5
            "            #DDMMDDDDMD#        ", // 6
            "           #DDMMDDDDMDDD#       ", // 7
            "          #DMDDDDDMMDDDD##      ", // 8
            "          #DDDD#DDDDDD#..#      ", // 9
            "          #DMD#.......#..#      ", // 10
            "          #DD#.........##       ", // 11
            "           ##...#...###         ", // 12
            "            #..M####            ", // 13
            "         ####.M#                ", // 14
            "       ##MM##M######            ", // 15
            "      #MMMM##MMMMMM##           ", // 16
            "     #MMDMM#.#MMMMM..#          ", // 17
            "     #MDDMM#.#MMMMMDM#          ", // 18
            "     #MMMMD#.#MMDMMM##          ", // 19
            "     #DMMMD#.#DMMMMM#.#         ", // 20
            "     #DDMDD#.#MMMDMM#.##        ", // 21
            "      #MDDD#.##DMMMD#..#        ", // 22
            "       #####..#DDDDD####        ", // 23
            "           #..#DDDDD#           ", // 24
            "          #DD##DDDD#            ", // 25
            "         #DD#  #DDDD#           ", // 26
            "        #DD#    #DDDD#          ", // 27
            "       #DM#      #DDDD#         ", // 28 Left knee kicking high
            "       #DD#       #DDDD#        ", // 29
            "       #DD#        #DDDD#       ", // 30
            "       #DM#         #DDDD#      ", // 31
            "       #DD#         ##...##     ", // 32 Left foot extending
            "       #DD#        #D....#      ", // 33
            "       #DM#        #DDMM.#      ", // 34
            "       #DD#        #######      ", // 35 Left sole at Y=12
            "       #DD#                     ", // 36
            "       #DD#                     ", // 37
            "      ##..##                    ", // 38
            "     #D....#                    ", // 39
            "     #DDMM.#                    ", // 40
            "     #.#####                    ", // 41 Right foot pushing back
            "     ###                        ", // 42
            "     ###                        ", // 43 GROUND LINE Y=4 (toe push)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Run 3: Left foot heel strike forward, right leg high trail
        public static readonly string[] Run3 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "               ######           ", // 5
            "             ##DDDDDD##         ", // 6
            "            #DDMMDDDDMD#        ", // 7
            "           #DDMMDDDDMDDD#       ", // 8
            "          #DMDDDDDMMDDDD##      ", // 9
            "          #DDDD#DDDDDD#..#      ", // 10
            "          #DMD#.......#..#      ", // 11
            "          #DD#.........##       ", // 12
            "           ##...#...###         ", // 13
            "            #..M####            ", // 14
            "         ####.M#                ", // 15
            "       ##MM##M######            ", // 16
            "      #MMMM##MMMMMM##           ", // 17
            "     #MMDMM#.#MMMMM..#          ", // 18 Right arm pumping forward
            "     #MDDMM#.#MMMMMDM#          ", // 19
            "     #MMMMD#.#MMDMMM##..#       ", // 20 Hand forward (.)
            "     #DMMMD#.#DMMMMM#..#        ", // 21
            "     #DDMDD#.#MMMDMM#.##        ", // 22
            "      #MDDD#.##DMMMD#           ", // 23
            "   #   #####..#DDDDD####        ", // 24 Left arm back
            "  #.#      #..#DDDDD#           ", // 25
            "   #       #DD#DDDDD#           ", // 26
            "          #DD# #DDDDD#          ", // 27 Opposite stride
            "         #DD#   #DDDDD#         ", // 28
            "        #DM#     #DDMDD#        ", // 29
            "       #DD#       #DMDDD#       ", // 30
            "      #DD#         #DDDDD#      ", // 31
            "  ### #DM#          #DDMDD#     ", // 32 Right leg high trail
            " #D..##DD#           #DDDDD#    ", // 33 Left foot reaching forward
            " #DDM##DD#            #DMDDD#   ", // 34
            " ##### #D#             #DDDDD#  ", // 35 Right foot high in back
            "                       #DDMDD#  ", // 36
            "                       #DDDDD#  ", // 37
            "                       #DDDDD#  ", // 38
            "                      ##...##   ", // 39
            "                     #D....#    ", // 40
            "                     #DDMM.#    ", // 41
            "                     #######    ", // 42 Left heel strike
            "                     #######    ", // 43 GROUND LINE Y=4 (heel contact)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Run 4: Left leg compression / weight bearing, right knee drives
        public static readonly string[] Run4 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5 Body dips 1px
            "               ######           ", // 6
            "             ##DDDDDD##         ", // 7
            "            #DDMMDDDDMD#        ", // 8
            "           #DDMMDDDDMDDD#       ", // 9
            "          #DMDDDDDMMDDDD##      ", // 10
            "          #DDDD#DDDDDD#..#      ", // 11
            "          #DMD#.......#..#      ", // 12
            "          #DD#.........##       ", // 13
            "           ##...#...###         ", // 14
            "            #..M####            ", // 15
            "         ####.M#                ", // 16
            "       ##MM##M######            ", // 17
            "      #MMMM##MMMMMM##           ", // 18
            "     #MMDMM#.#MMMMM..#          ", // 19
            "     #MDDMM#.#MMMMMDM#          ", // 20
            "     #MMMMD#.#MMDMMM##          ", // 21
            "     #DMMMD#.#DMMMMM#.#         ", // 22
            "     #DDMDD#.#MMMDMM#.##        ", // 23
            "      #MDDD#.##DMMMD#..#        ", // 24
            "       #####..#DDDDD####        ", // 25
            "           #..#DDDDD#           ", // 26
            "           #DD#DDDDD#           ", // 27 Right leg bending
            "          #DD# #DDDDD#          ", // 28
            "         #DD#   #DDMDD#         ", // 29
            "        #DD#     #DMDDD#        ", // 30
            "       #DM#       #DDDDD#       ", // 31
            "      ##..#       #DDMDD#       ", // 32 Right foot passing
            "     #D...#       #DDDDD#       ", // 33
            "     #DDMM#       #DMDDD#       ", // 34
            "     ######       #DDDDD#       ", // 35 Right sole at Y=12
            "                  #DDMDD#       ", // 36
            "                  #DDDDD#       ", // 37
            "                  #DDDDD#       ", // 38
            "                 ##...##        ", // 39
            "                #D....#         ", // 40
            "                #DDMM.#         ", // 41
            "                #######         ", // 42 Left foot fully planted
            "                #######         ", // 43 GROUND LINE Y=4 (full plant)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Run 5: Left leg push-off / propulsion, right leg swings forward
        public static readonly string[] Run5 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "               ######           ", // 4 Apex rise
            "             ##DDDDDD##         ", // 5
            "            #DDMMDDDDMD#        ", // 6
            "           #DDMMDDDDMDDD#       ", // 7
            "          #DMDDDDDMMDDDD##      ", // 8
            "          #DDDD#DDDDDD#..#      ", // 9
            "          #DMD#.......#..#      ", // 10
            "          #DD#.........##       ", // 11
            "           ##...#...###         ", // 12
            "            #..M####            ", // 13
            "         ####.M#                ", // 14
            "       ##MM##M######            ", // 15
            "      #MMMM##MMMMMM##           ", // 16
            "     #MMDMM#.#MMMMM..#          ", // 17
            "     #MDDMM#.#MMMMMDM#          ", // 18
            "     #MMMMD#.#MMDMMM##          ", // 19
            "     #DMMMD#.#DMMMMM#.#         ", // 20
            "     #DDMDD#.#MMMDMM#.##        ", // 21
            "      #MDDD#.##DMMMD#..#        ", // 22
            "       #####..#DDDDD####        ", // 23
            "           #..#DDDDD#           ", // 24
            "          #DD##DDDD#            ", // 25
            "         #DD#  #DDDD#           ", // 26
            "        #DD#    #DDDD#          ", // 27
            "       #DM#      #DDDD#         ", // 28 Right knee kicking high
            "       #DD#       #DDDD#        ", // 29
            "       #DD#        #DDDD#       ", // 30
            "       #DM#         #DDDD#      ", // 31
            "       #DD#         ##...##     ", // 32 Right foot extending
            "       #DD#        #D....#      ", // 33
            "       #DM#        #DDMM.#      ", // 34
            "       #DD#        #######      ", // 35 Right sole at Y=12
            "       #DD#                     ", // 36
            "       #DD#                     ", // 37
            "      ##..##                    ", // 38
            "     #D....#                    ", // 39
            "     #DDMM.#                    ", // 40
            "     #.#####                    ", // 41 Left foot pushing back
            "     ###                        ", // 42
            "     ###                        ", // 43 GROUND LINE Y=4 (toe push)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // ==========================================
        // CROUCH IDLE (2 frames, lowered profile ~24px tall, head below duct ceiling)
        // ==========================================

        // Crouch Idle 0: Hunched low, knees bent, hand on knee, top at Row 20 (y=27)
        public static readonly string[] CrouchIdle0 = new string[]
        {
            "                                ", // 0 CLEAR DUCT CEILING
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5
            "                                ", // 6
            "                                ", // 7
            "                                ", // 8
            "                                ", // 9
            "                                ", // 10
            "                                ", // 11
            "                                ", // 12
            "                                ", // 13
            "                                ", // 14
            "                                ", // 15
            "                                ", // 16
            "                                ", // 17
            "                                ", // 18
            "                 ######         ", // 19 (y=28) Head lowered dramatically
            "       ####### ##DDDDDD##       ", // 20 (y=27) Backpack + Hair
            "      #MMMMMMM#DDMMDDDDMD#      ", // 21 (y=26)
            "     #MMMDMMMM#DDDDDD#..##      ", // 22 (y=25) Face brow / nose
            "    #MMDDMMMMM#.......#..#      ", // 23 (y=24) Eye pupil
            "    #MMMMMMDMM#........##       ", // 24 (y=23) Chin
            "    #MDMMMMMMMM##...###         ", // 25 (y=22) Neck
            "    #DDMDMMMMMMM#..M####        ", // 26 (y=21) Chest
            "     #MDDDMMMMMM#.M#            ", // 27 (y=20) Torso
            "      #####MMMMM#M######        ", // 28 (y=19) Low back
            "          #DDDD#MMMMMM##        ", // 29 (y=18)
            "         #DDDDD#.#MMMM..#       ", // 30 (y=17) Hand resting on knee (.)
            "        #DDDDDD#.#MMMD#..#      ", // 31 (y=16)
            "       #DDDDDDD#.##DDD####      ", // 32 (y=15)
            "       #DDDDDDD#  #DDDDD#       ", // 33 (y=14) Hunched pelvis
            "      #DDDDDDDD#  #DDDDD#       ", // 34 (y=13) Bent thighs
            "     #DDDDD#DDD#  #DDMDD#       ", // 35 (y=12) Bent knees
            "     #DDDD# #DD#  #DMDDD#       ", // 36 (y=11)
            "     #DDDD# #DD#  #DDDDD#       ", // 37 (y=10)
            "     #DDDD# #DD#  #DDDDD#       ", // 38 (y=9)
            "    ##...## ##..###...##        ", // 39 (y=8)  Crouch sneaker tongues
            "   #D....# #D....#D....#        ", // 40 (y=7)  Sneakers
            "   #DDMM.# #DDMM.#DDMM.#        ", // 41 (y=6)  Sneakers
            "   ####### #######.#####        ", // 42 (y=5)  Sneaker soles
            "   ####### #######.#####        ", // 43 (y=4)  GROUND LINE Y=4 (both feet flat)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Crouch Idle 1: Subtle breathing pulse in crouch
        public static readonly string[] CrouchIdle1 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5
            "                                ", // 6
            "                                ", // 7
            "                                ", // 8
            "                                ", // 9
            "                                ", // 10
            "                                ", // 11
            "                                ", // 12
            "                                ", // 13
            "                                ", // 14
            "                                ", // 15
            "                                ", // 16
            "                                ", // 17
            "                                ", // 18
            "                 ######         ", // 19 (y=28)
            "       ####### ##DDDDDD##       ", // 20 (y=27)
            "      #MMMMMMM#DDMMDDDDMD#      ", // 21 (y=26)
            "     #MMMDMMMM#DDDDDD#..##      ", // 22 (y=25)
            "    #MMDDMMMMM#.......#..#      ", // 23 (y=24) Eye blink / glance
            "    #MMMMMMDMM#........##       ", // 24 (y=23)
            "    #MDMMMMMMMM##...###         ", // 25 (y=22)
            "    #DDMDMMMMMMM#..M####        ", // 26 (y=21)
            "     #MDDDMMMMMM#.M#            ", // 27 (y=20)
            "      #####MMMMM#M######        ", // 28 (y=19)
            "          #DDDD#MMMMMM##        ", // 29 (y=18)
            "         #DDDDD#.#MMMM..#       ", // 30 (y=17)
            "        #DDDDDD#.#MMMD#..#      ", // 31 (y=16)
            "       #DDDDDDD#.##DDD####      ", // 32 (y=15)
            "       #DDDDDDD#  #DDDDD#       ", // 33 (y=14)
            "      #DDDDDDDD#  #DDDDD#       ", // 34 (y=13)
            "     #DDDDD#DDD#  #DDMDD#       ", // 35 (y=12)
            "     #DDDD# #DD#  #DMDDD#       ", // 36 (y=11)
            "     #DDDD# #DD#  #DDDDD#       ", // 37 (y=10)
            "     #DDDD# #DD#  #DDDDD#       ", // 38 (y=9)
            "    ##...## ##..###...##        ", // 39 (y=8)
            "   #D....# #D....#D....#        ", // 40 (y=7)
            "   #DDMM.# #DDMM.#DDMM.#        ", // 41 (y=6)
            "   ####### #######.#####        ", // 42 (y=5)
            "   ####### #######.#####        ", // 43 (y=4)  GROUND LINE Y=4 (exact same foot base)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // ==========================================
        // CROUCH WALK (4 frames, crawling low under ducts)
        // ==========================================

        // Crouch Walk 0: Right knee forward, left leg extended back creeping
        public static readonly string[] CrouchWalk0 = new string[]
        {
            "                                ", // 0 CLEAR DUCT CEILING
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5
            "                                ", // 6
            "                                ", // 7
            "                                ", // 8
            "                                ", // 9
            "                                ", // 10
            "                                ", // 11
            "                                ", // 12
            "                                ", // 13
            "                                ", // 14
            "                                ", // 15
            "                                ", // 16
            "                                ", // 17
            "                                ", // 18
            "                 ######         ", // 19 (y=28)
            "       ####### ##DDDDDD##       ", // 20 (y=27)
            "      #MMMMMMM#DDMMDDDDMD#      ", // 21 (y=26)
            "     #MMMDMMMM#DDDDDD#..##      ", // 22 (y=25)
            "    #MMDDMMMMM#.......#..#      ", // 23 (y=24)
            "    #MMMMMMDMM#........##       ", // 24 (y=23)
            "    #MDMMMMMMMM##...###         ", // 25 (y=22)
            "    #DDMDMMMMMMM#..M####        ", // 26 (y=21)
            "     #MDDDMMMMMM#.M#            ", // 27 (y=20)
            "      #####MMMMM#M######        ", // 28 (y=19)
            "          #DDDD#MMMMMM##        ", // 29 (y=18) Hand reaching forward
            "         #DDDDD#.#MMMM..#       ", // 30 (y=17)
            "        #DDDDDD#.#MMMD#..#      ", // 31 (y=16) Hand on ground
            "       #DDDDDDD#.##DDD####      ", // 32 (y=15)
            "      #DDDD####   #DDDDD#       ", // 33 (y=14) Stride separation
            "     #DDDD#        #DDDDD#      ", // 34 (y=13)
            "    #DDDD#          #DDMDD#     ", // 35 (y=12)
            "   #DDDD#            #DMDDD#    ", // 36 (y=11)
            "   #DDDD#            #DDDDD#    ", // 37 (y=10)
            "  ##...##            #DDDDD#    ", // 38 (y=9)
            " #D....#            ##...##     ", // 39 (y=8)
            " #DDMM.#           #D....#      ", // 40 (y=7)
            " #######           #DDMM.#      ", // 41 (y=6)
            " #######           #######      ", // 42 (y=5)
            " #######           #######      ", // 43 (y=4) GROUND LINE Y=4 (both feet touch)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Crouch Walk 1: Shifting weight, left foot pushing off
        public static readonly string[] CrouchWalk1 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5
            "                                ", // 6
            "                                ", // 7
            "                                ", // 8
            "                                ", // 9
            "                                ", // 10
            "                                ", // 11
            "                                ", // 12
            "                                ", // 13
            "                                ", // 14
            "                                ", // 15
            "                                ", // 16
            "                                ", // 17
            "                                ", // 18
            "                  ######        ", // 19 Body forward creep
            "        ####### ##DDDDDD##      ", // 20
            "       #MMMMMMM#DDMMDDDDMD#     ", // 21
            "      #MMMDMMMM#DDDDDD#..##     ", // 22
            "     #MMDDMMMMM#.......#..#     ", // 23
            "     #MMMMMMDMM#........##      ", // 24
            "     #MDMMMMMMMM##...###        ", // 25
            "     #DDMDMMMMMMM#..M####       ", // 26
            "      #MDDDMMMMMM#.M#           ", // 27
            "       #####MMMMM#M######       ", // 28
            "           #DDDD#MMMMMM##       ", // 29
            "          #DDDDD#.#MMMM..#      ", // 30
            "         #DDDDDD#.#MMMD#..#     ", // 31
            "        #DDDDDDD#.##DDD####     ", // 32
            "       #DDDD####   #DDDDD#      ", // 33
            "      #DDDD#        #DDDDD#     ", // 34
            "     #DDDD#          #DDMDD#    ", // 35
            "     ##..##          #DMDDD#    ", // 36 Left toe lifting
            "    #D....#          #DDDDD#    ", // 37
            "    #DDMM.#          #DDDDD#    ", // 38
            "    #######         ##...##     ", // 39
            "    ###            #D....#      ", // 40
            "    ###            #DDMM.#      ", // 41
            "    ###            #######      ", // 42
            "    ###            #######      ", // 43 GROUND LINE Y=4 (right planted, left pushing)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Crouch Walk 2: Left knee forward, right leg extended back creeping
        public static readonly string[] CrouchWalk2 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5
            "                                ", // 6
            "                                ", // 7
            "                                ", // 8
            "                                ", // 9
            "                                ", // 10
            "                                ", // 11
            "                                ", // 12
            "                                ", // 13
            "                                ", // 14
            "                                ", // 15
            "                                ", // 16
            "                                ", // 17
            "                                ", // 18
            "                 ######         ", // 19 (y=28)
            "       ####### ##DDDDDD##       ", // 20 (y=27)
            "      #MMMMMMM#DDMMDDDDMD#      ", // 21 (y=26)
            "     #MMMDMMMM#DDDDDD#..##      ", // 22 (y=25)
            "    #MMDDMMMMM#.......#..#      ", // 23 (y=24)
            "    #MMMMMMDMM#........##       ", // 24 (y=23)
            "    #MDMMMMMMMM##...###         ", // 25 (y=22)
            "    #DDMDMMMMMMM#..M####        ", // 26 (y=21)
            "     #MDDDMMMMMM#.M#            ", // 27 (y=20)
            "      #####MMMMM#M######        ", // 28 (y=19)
            "          #DDDD#MMMMMM##        ", // 29 (y=18) Opposite hand reaching
            "         #DDDDD#.#MMMM..#       ", // 30 (y=17)
            "        #DDDDDD#.#MMMD#..#      ", // 31 (y=16) Hand on ground
            "       #DDDDDDD#.##DDD####      ", // 32 (y=15)
            "       #DDDDD#   ####DDDD#      ", // 33 (y=14) Opposite leg split
            "      #DDDDD#        #DDDD#     ", // 34 (y=13)
            "     #DDMDD#          #DDDD#    ", // 35 (y=12)
            "    #DMDDD#            #DDDD#   ", // 36 (y=11)
            "    #DDDDD#            #DDDD#   ", // 37 (y=10)
            "    #DDDDD#            ##...##  ", // 38 (y=9)
            "   ##...##            #D....#   ", // 39 (y=8)
            "  #D....#             #DDMM.#   ", // 40 (y=7)
            "  #DDMM.#             #######   ", // 41 (y=6)
            "  #######             #######   ", // 42 (y=5)
            "  #######             #######   ", // 43 (y=4) GROUND LINE Y=4 (both feet touch)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };

        // Crouch Walk 3: Shifting weight, right foot pushing off
        public static readonly string[] CrouchWalk3 = new string[]
        {
            "                                ", // 0
            "                                ", // 1
            "                                ", // 2
            "                                ", // 3
            "                                ", // 4
            "                                ", // 5
            "                                ", // 6
            "                                ", // 7
            "                                ", // 8
            "                                ", // 9
            "                                ", // 10
            "                                ", // 11
            "                                ", // 12
            "                                ", // 13
            "                                ", // 14
            "                                ", // 15
            "                                ", // 16
            "                                ", // 17
            "                                ", // 18
            "                  ######        ", // 19 Body forward creep
            "        ####### ##DDDDDD##      ", // 20
            "       #MMMMMMM#DDMMDDDDMD#     ", // 21
            "      #MMMDMMMM#DDDDDD#..##     ", // 22
            "     #MMDDMMMMM#.......#..#     ", // 23
            "     #MMMMMMDMM#........##      ", // 24
            "     #MDMMMMMMMM##...###        ", // 25
            "     #DDMDMMMMMMM#..M####       ", // 26
            "      #MDDDMMMMMM#.M#           ", // 27
            "       #####MMMMM#M######       ", // 28
            "           #DDDD#MMMMMM##       ", // 29
            "          #DDDDD#.#MMMM..#      ", // 30
            "         #DDDDDD#.#MMMD#..#     ", // 31
            "        #DDDDDDD#.##DDD####     ", // 32
            "        #DDDDD#   ####DDDD#     ", // 33
            "       #DDDDD#        #DDDD#    ", // 34
            "      #DDMDD#          #DDDD#   ", // 35
            "      #DMDDD#          ##..##   ", // 36 Right toe lifting
            "      #DDDDD#         #D....#   ", // 37
            "      #DDDDD#         #DDMM.#   ", // 38
            "     ##...##          #######   ", // 39
            "    #D....#           ###       ", // 40
            "    #DDMM.#           ###       ", // 41
            "    #######           ###       ", // 42
            "    #######           ###       ", // 43 GROUND LINE Y=4 (left planted, right pushing)
            "                                ", // 44
            "                                ", // 45
            "                                ", // 46
            "                                "  // 47
        };
    }
}
