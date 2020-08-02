using System;
using System.Collections.Generic;
using ROI.Tiles.Wasteland;
using ROI.Walls.Wasteland;
using ROI.Worlds.Structures;
using ROI.Worlds.Structures.Wasteland;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using static Terraria.WorldGen;

namespace ROI.Worlds
{
    //Contain wasteland world gen
    internal partial class ROIWorld : ModWorld
    {

        internal static Dictionary<int, int> SurfaceLevel;
        internal static int HighestLevel = 300;

        internal static int AmountOfBigLakePerWorld
        {
            get
            {
                switch (Main.maxTilesX)
                {
                    case 4200:
                        return 2;
                    case 6400:
                        return 3;
                    case 8400:
                        return 3;
                    case 16800:
                        return 5;
                    default:
                        return 1;

                }
            }
        }

        public static int[] GetPerlinDisplacements(int displacementCount, float frequency, int maxLimit, float multiplier, int seed)
        {
            FastNoise noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            noise.SetFrequency(frequency);

            int[] displacements = new int[displacementCount];

            for (int x = 0; x < displacementCount; x++)
                displacements[x] = (int)Math.Floor(noise.GetNoise(x, x) * maxLimit * multiplier);

            return displacements;
        }

        internal void Fill(int x, int startingY, int one, int depth, ushort tile)
        {
            for (int i = startingY; i < startingY + depth; i++)
            {
                if (!WorldGen.InWorld(x, i))
                {
                    return;
                }
                WorldGen.PlaceTile(x, i, tile, false, true);
            }
        }

        internal void FillAir(int x, int depth, int one, int startingY)
        {
            for (int i = startingY; i < startingY + depth; i++)
            {
                Main.tile[x, i].active(false);
            }
        }

        internal void WastelandGeneration(GenerationProgress progress)
        {
            ClearTerrain();
            BaseTerrain(new float[] {0.00077f, 0.00011f, 0.022f}, new float[] {0.3f, 0.05f, 0.02f});
            BaseTerrain(new float[] {0.0001f, 0.00011f, 0.01f}, new float[] {0.2f, 0.02f, 0.02f}, true);
            progress.Message = "Accumulating waste";
            //actual world gen
            /*if (WorldGen.gen)
            {
                for (int i = 0; i < 10; i++)
                {
                    TerrainTop(progress);
                }
            }
            //Debug stick gen
            else
            {
                TerrainTop(progress);
            }*/
            //TerrainBottom(progress);
            StructureMap map = new StructureMap();
            
            SpreadingGrass(progress);
            GenerateCavern(progress);
            GenerateWasteLake(progress);
            WastelandOreGeneration(progress);
            GeneratingRuins(progress);
            WastelandForgeGen(progress);
            //GrowingTree(progress);
            
            //GenerateMysteriousGrotto(progress);
            WorldGen.EveryTileFrame();
        }

        private void BaseTerrain(float[] freq, float[] limit, bool top = false)
        {
            int[][] displacements = new int[freq.Length][];
            for (int i = 0; i < freq.Length; i++)
            {
                displacements[i] = GetPerlinDisplacements(Main.maxTilesX, freq[i], Main.maxTilesY - 150, limit[i], WorldGen._lastSeed);
            }

            int[] totalDisplacements = new int[Main.maxTilesX];

            for (int i = 0; i < displacements.Length; i++)
            {
                for (int j = 0; j < Main.maxTilesX; j++)
                {
                    totalDisplacements[j] += displacements[i][j];
                }
            }

            SurfaceLevel = new Dictionary<int, int>();
            if (!top)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    totalDisplacements[i] = (int) (totalDisplacements[i] / displacements.Length + (Main.maxTilesY - 125));
                    SurfaceLevel[i] = totalDisplacements[i];

                    if (totalDisplacements[i] < HighestLevel || HighestLevel == 0)
                    {
                        HighestLevel = totalDisplacements[i];
                    }

                    int dirtDepth = WorldGen.genRand.Next(10, 15);
                    Fill(i, totalDisplacements[i], 1, dirtDepth, (ushort) mod.TileType("Wasteland_Dirt"));
                    Fill(i, totalDisplacements[i] + dirtDepth, 1, 200, (ushort) mod.TileType("Wasteland_Rock"));
                    FillAir(i, 0, 1, totalDisplacements[i]);
                }
            }
            else
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    totalDisplacements[i] = (int) (totalDisplacements[i] / displacements.Length + (Main.maxTilesY - 225));
                    SurfaceLevel[i] = totalDisplacements[i];

                    if (totalDisplacements[i] < HighestLevel || HighestLevel == 0)
                    {
                        HighestLevel = totalDisplacements[i];
                    }

                    int dirtDepth = WorldGen.genRand.Next(10, 15);
                    Fill(i, totalDisplacements[i], 1, dirtDepth, (ushort) mod.TileType("Wasteland_Dirt"));
                }
            }
            

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                    WorldGen.TileRunner(i, SurfaceLevel[i] + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(14, 20), WorldGen.genRand.Next(20, 28), -1, true);
            }
        }

        private static void ClearTerrain()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j] == null)
                    {
                        Main.tile[i, j] = new Tile();
                    }
                    Main.tile[i, j].active(false);
                    Main.tile[i, j].liquid = 0;
                    Main.tile[i, j].wall = 0;
                }
            }
        }

        private static void WastelandOreGeneration(GenerationProgress progress)
        {
            for (int num11 = 0; num11 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num11++)
            {
                if(WorldGen.genRand.Next(20) == 0)
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), ModContent.TileType<Wasteland_Ore>(), false, 0f, 0f, false, true);
            }
        }

        private void GenerateWasteLake(GenerationProgress progress)
        {
            int smallLake = 0;
            int bigLake = 0;

            for (int i = 0; i < 25; i++)
            {
                int x = Main.rand.Next(200, Main.maxTilesX - 200);
                int yGen = 0;
                if (Scan(x, Main.maxTilesY - 175, out yGen, (tile =>
                {
                    if (tile.type != ModContent.TileType<Wasteland_Dirt>() || tile.liquid > 0)
                        return false;
                    return true;
                })))
                {
                    if (bigLake != AmountOfBigLakePerWorld)
                    {
                        WastelandLake.Generate(x, yGen, 40, 55, 8f, 3, 3, false);
                        bigLake++;
                        continue;
                    }
                    WastelandLake.Generate(x, yGen, 20, 24, 5f, 3, 3, true);

                }
            }
        }



        private bool Scan(int x, int y, out int yGen, Func<Tile, bool> condition = null)
        {
            for (int i = y; i < Main.maxTilesX; i++)
            {
                if (!WorldGen.InWorld(x, i))
                {
                    yGen = 0;
                    return false;
                }
                if (!Main.tile[x, i].active() && Main.tile[x, i].liquid == 0)
                {
                    continue;
                }
                else
                {
                    if (condition.Invoke(Main.tile[x, i]))
                    {
                        yGen = i;
                        return true;
                    }
                }
            }

            yGen = -1;
            return false;
        }

        private void GeneratingRuins(GenerationProgress progress)
        {
            int nextHouseCooldown = 0;
            for (int i = (int)(Main.maxTilesX * 0.30); i < Main.maxTilesX * 0.70; i++)
            {
                if (nextHouseCooldown <= 0 && WorldGen.genRand.Next(5) == 0)
                {
                    for (int j = Main.maxTilesY - 120; j < Main.maxTilesY - 20; j++)
                    {
                        if (Main.tile[i, j].type == mod.TileType("Wasteland_Dirt"))
                        {
                            nextHouseCooldown = WorldGen.genRand.Next(100, 150);
                            WastelandRuins.PlaceHouse(i, j);
                            break;
                        }
                    }

                }
                nextHouseCooldown--;
            }
        }

        private void SpreadingGrass(GenerationProgress progress)
        {
            ushort dirtType = (ushort)mod.TileType("Wasteland_Dirt");
            ushort grassType = (ushort)mod.TileType("Wasteland_Grass");
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                float percent = (float)(i / Main.maxTilesX);
                progress.Set(percent);
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type == dirtType && !Main.tile[i, j - 1].active())
                    {
                        Main.tile[i, j].wall = 0;
                        WorldGen.SpreadGrass(i, j, dirtType, grassType, true);
                    }
                }
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                float percent = (float)(i / Main.maxTilesX);
                progress.Set(percent);
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type == grassType)
                    {
                        Main.tile[i, j].wall = 0;
                    }
                }
            }
        }

        private void GenerateCavern(GenerationProgress progress)
        {
            int MAX_WASTELAND_HEIGHT = Main.maxTilesY - 130;

            progress.Message = "Digging the wasteland for research";
            for (int i = 0; i < Main.maxTilesX * 0.15; i++)
            {
                float percent = (float)(i / Main.maxTilesX * 0.15);
                progress.Set(percent);
                int type = -1;

                ROIWorldHelper.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(MAX_WASTELAND_HEIGHT, Main.maxTilesY), (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(50, 70), type);
                ROIWorldHelper.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(MAX_WASTELAND_HEIGHT, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(20, 50), mod.TileType("Wasteland_Dirt"));
            }

        }

        internal void TerrainTop(GenerationProgress progress)
        {
            //Create the top of the wasteland
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY - 195; j++)
                {
                    if (WorldGen.genRand.Next(20) == 0)
                    {
                        WorldGen.TileRunner(i, j, (double)WorldGen.genRand.Next(4, 6), 50, mod.TileType("Wasteland_Rock"), true);
                    }
                }
            }

            //then get rid of the bottom of the world, wasteland rock will need an evil pick to be mined
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type != mod.TileType("Wasteland_Rock"))
                    {
                        Main.tile[i, j].active(false);
                        Main.tile[i, j].liquid = 0;
                        Main.tile[i, j].wall = 0;
                        Main.tile[i, j].slope(0);
                    }
                }
            }
        }


        internal void TerrainBottom(GenerationProgress progress)
        {
            int MAX_WASTELAND_HEIGHT = Main.maxTilesY - 130;
            int MIN_WASTELAND_HEIGHT = Main.maxTilesY - 90;
            int MAX_WASTELAND_ROCK_HEIGHT = Main.maxTilesY - 90;
            int MIN_WASTELAND_ROCK_HEIGHT = Main.maxTilesY - 70;

            int wastelandHeight = Main.maxTilesY - 100;
            int wastelandRock = Main.maxTilesY - 85;

            int heightModifierBasedOnXAxis = 0;
            int nextModifierCoolDown = WorldGen.genRand.Next(20, 50);

            for (int i = 0; i < Main.maxTilesX; i++)
            {

                if (nextModifierCoolDown <= 0)
                {
                    heightModifierBasedOnXAxis = WorldGen.genRand.Next(0, 6);
                    nextModifierCoolDown = WorldGen.genRand.Next(20, 50);
                    if (heightModifierBasedOnXAxis == 0)
                    {
                        nextModifierCoolDown = (int)(WorldGen.genRand.Next(20, 50) * 0.5);
                    }
                }

                nextModifierCoolDown--;

                if (i > Main.maxTilesX * 0.40 && i < Main.maxTilesX * 0.60)
                {
                    //heightModifierBasedOnXAxis = 4;
                }

                switch (heightModifierBasedOnXAxis)
                {
                    case 0:
                        if (WorldGen.genRand.Next(5) == 0)
                        {
                            wastelandHeight += 1;
                        }
                        break;
                    case 1:
                        if (WorldGen.genRand.Next(5) == 0)
                        {
                            wastelandHeight -= 1;
                        }
                        break;
                    case 2:
                        if (WorldGen.genRand.Next(6) == 0)
                        {
                            wastelandRock -= 2;
                        }
                        break;
                    case 3:
                        if (WorldGen.genRand.Next(6) == 0)
                        {
                            wastelandRock += 2;
                        }
                        break;
                    case 4:
                        if (WorldGen.genRand.Next(3) == 0)
                        {
                            wastelandHeight -= WorldGen.genRand.Next(-2, 1);
                        }
                        break;
                }

                if (wastelandHeight <= MAX_WASTELAND_HEIGHT)
                {
                    wastelandHeight = MAX_WASTELAND_HEIGHT + WorldGen.genRand.Next(4, 6);
                }

                if (wastelandHeight >= MIN_WASTELAND_HEIGHT)
                {
                    wastelandHeight = MIN_WASTELAND_HEIGHT - WorldGen.genRand.Next(4, 6);
                }

                if (wastelandRock <= MAX_WASTELAND_ROCK_HEIGHT)
                {
                    wastelandRock = MAX_WASTELAND_ROCK_HEIGHT + WorldGen.genRand.Next(4, 6);
                }

                if (wastelandRock >= MIN_WASTELAND_ROCK_HEIGHT)
                {
                    wastelandRock = MIN_WASTELAND_ROCK_HEIGHT - WorldGen.genRand.Next(4, 6);
                }

                for (int j = wastelandHeight; j < wastelandRock; j++)
                {
                    Main.tile[i, j].active(true);
                    Main.tile[i, j].slope(0);
                    Main.tile[i, j].type = (ushort)mod.TileType("Wasteland_Dirt");
                    Main.tile[i, j].wall = (ushort)mod.WallType("Wasteland_Dirt_Wall");
                }

                for (int j = wastelandRock; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].active(true);
                    Main.tile[i, j].slope(0);
                    Main.tile[i, j].type = (ushort)mod.TileType("Wasteland_Rock");
                    Main.tile[i, j].wall = (ushort)mod.WallType("Wasteland_Rock_Wall");
                }
            }
        }

        private static void WastelandForgeGen(GenerationProgress progress)
        {
            progress.Message = "Adding wasteland forge";
            for (int num258 = 0; num258 < Main.maxTilesX / 200; num258++)
            {
                float value = num258 / (Main.maxTilesX / 200);
                progress.Set(value);
                bool flag17 = false;
                int num259 = 0;
                while (!flag17)
                {
                    int num260 = genRand.Next(1, Main.maxTilesX);
                    int num261 = genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 5);
                    try
                    {
                        if (Main.tile[num260, num261].wall == ModContent.WallType<WastestoneBrickWall>())
                        {
                            for (; !Main.tile[num260, num261].active(); num261++)
                            {
                            }

                            num261--;
                            PlaceTile(num260, num261, ModContent.TileType<Wasteland_Forge>());
                            if (Main.tile[num260, num261].type == ModContent.TileType<Wasteland_Forge>())
                            {
                                flag17 = true;
                            }
                            else
                            {
                                num259++;
                                if (num259 >= 10000)
                                    flag17 = true;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
