using Microsoft.Xna.Framework;
using ROI.Content.Subworlds.Wasteland.Furniture.Walls;
using ROI.Content.Subworlds.Wasteland.WorldBuilding.Helpers;
using ROI.Content.Subworlds.Wasteland.WorldBuilding.Tiles;
using ROI.Worlds;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using static Terraria.WorldGen;

namespace ROI.Content.Subworlds.Wasteland
{
    //Contain wasteland world gen
    internal static class WastelandWorldBuilding
    {
        internal static Dictionary<int, int> SurfaceLevel;
        internal static int HighestLevel = 300;
        internal static Mod Mod => ModContent.GetInstance<ROIMod>();

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

        public static int[] GetPerlinDisplacements(int displacementCount, float frequency, int maxLimit, float multiplier, int octave, int seed)
        {
            FastNoise noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            noise.SetFrequency(frequency);
            noise.SetFractalOctaves(octave);

            int[] displacements = new int[displacementCount];

            for (int x = 0; x < displacementCount; x++)
                displacements[x] = (int)Math.Floor(noise.GetNoise(x, x) * maxLimit * multiplier);
            return displacements;
        }

        public static int[,] GetPerlinDisplacements2D(int width, int height, float frequency, int maxLimit, float multiplier, int octave, int seed)
        {
            FastNoise noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            noise.SetFrequency(frequency);
            noise.SetFractalOctaves(octave);

            int[,] displacements = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    displacements[x, y] = (int)Math.Floor(noise.GetNoise(x, y) * maxLimit * multiplier);
                }
            }
            return displacements;
        }

        public static FastNoise GeneratePerlin(float gain, float lucanarity, float frequency, int octave, int seed)
        {
            FastNoise noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            noise.SetFrequency(frequency);
            noise.SetFractalOctaves(octave);

            return noise;
        }


        internal static void WastelandGeneration(GenerationProgress progress)
        {
            //Bottom
            BaseTerrain(new float[] { 0.0077f, 0.0011f, 0.022f, 0.04f }, new float[] { 0.7f, 0.05f, 0.02f, 0.01f }, new int[] { 5, 5, 5, 5 }, 450);
            //Top
            BaseTerrain(new float[] { 0.07f, 0.04f, 0.03f }, new float[] { 0.7f, 0.04f, 0.06f }, new int[] { 5, 5, 5, 5 }, 200, true);
            progress.Message = "Accumulating waste";
            //actual world gen
            StructureMap map = new StructureMap();

            SpreadingGrass(progress);
            GeneratePerlinCavern(progress);
            GenerateMossCavern(progress);
            GenerateWasteLake(progress);
            WastelandOreGeneration(progress);
            //GeneratingRuins(progress);
            //WastelandForgeGen(progress);
            WastelandUraniumOreGen(progress);
            //GrowingTree(progress);
            //GenerateMysteriousGrotto(progress);
        }

        private static void WastelandUraniumOreGen(GenerationProgress progress)
        {
            for (int i = 0; i < 50; i++)
            {
                int x = Main.rand.Next(25, Main.maxTilesX - 25);
                int yGen = 0;
                if (Scan(x, 300, out yGen, (tile => { return true; })))
                {
                    WorldUtils.Gen(new Point(x, yGen), new Shapes.Tail(WorldGen.genRand.Next(10, 13), new Vector2(WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(20, 25))), Actions.Chain(new GenAction[]
                    {
                    new Actions.PlaceTile((ushort) Mod.TileType("Uranium_Ore"), 0)
                    }));
                }
            }
        }

        private static void BaseTerrain(float[] freq, float[] limit, int[] octave, int startingHeight, bool top = false)
        {
            int[][] displacements = new int[freq.Length][];
            for (int i = 0; i < freq.Length; i++)
            {
                displacements[i] = GetPerlinDisplacements(Main.maxTilesX, freq[i], startingHeight, limit[i], octave[i], WorldGen._lastSeed);
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
                    totalDisplacements[i] = (int)(totalDisplacements[i] / displacements.Length + (startingHeight));
                    SurfaceLevel[i] = totalDisplacements[i];

                    if (totalDisplacements[i] < HighestLevel || HighestLevel == 0)
                    {
                        HighestLevel = totalDisplacements[i];
                    }

                    int dirtDepth = WorldGen.genRand.Next(10, 15);
                    GeneralWorldHelper.Fill(i, totalDisplacements[i], 1, dirtDepth, (ushort)ModContent.TileType<WastelandDirt>());
                    GeneralWorldHelper.Fill(i, totalDisplacements[i] + dirtDepth, 1, 900, (ushort)Mod.TileType("WastelandRock"));
                    //GeneralWorldHelper.FillAir(i, 0, 1, totalDisplacements[i]);
                }
            }
            else
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    totalDisplacements[i] = (int)(totalDisplacements[i] / displacements.Length + (startingHeight));
                    SurfaceLevel[i] = totalDisplacements[i];

                    if (totalDisplacements[i] < HighestLevel || HighestLevel == 0)
                    {
                        HighestLevel = totalDisplacements[i];
                    }

                    int dirtDepth = WorldGen.genRand.Next(10, 15);
                    GeneralWorldHelper.FillInvertYAxis(i, totalDisplacements[i], 1, 400, (ushort)Mod.TileType("WastelandRock"));
                }
            }


            for (int i = 0; i < Main.maxTilesX; i++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                    WorldGen.TileRunner(i, SurfaceLevel[i] + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(14, 20), WorldGen.genRand.Next(20, 28), -1, true);
            }
        }

        private static void WastelandOreGeneration(GenerationProgress progress)
        {
            for (int num11 = 0; num11 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num11++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(400, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), ModContent.TileType<WastelandOre>(), false, 0f, 0f, false, true);
            }
        }

        private static void GenerateWasteLake(GenerationProgress progress)
        {
            int smallLake = 0;
            int bigLake = 0;

            for (int i = 0; i < 25; i++)
            {
                int x = Main.rand.Next(200, Main.maxTilesX - 200);
                int yGen = 0;
                if (Scan(x, 200, out yGen, (tile =>
                {
                    if (tile.type != ModContent.TileType<WastelandDirt>() || tile.liquid > 0)
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



        private static bool Scan(int x, int y, out int yGen, Func<Tile, bool> condition = null)
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

        private static void GeneratingRuins(GenerationProgress progress)
        {
            int nextHouseCooldown = 0;
            for (int i = (int)(Main.maxTilesX * 0.30); i < Main.maxTilesX * 0.70; i++)
            {
                if (nextHouseCooldown <= 0 && WorldGen.genRand.Next(5) == 0)
                {
                    int j = 0;
                    if (Scan(i, 250, out j, tile =>
                    {
                        if (tile.type != ModContent.TileType<WastelandDirt>() || tile.liquid > 0)
                            return false;
                        return true;
                    }))
                    {
                        if (Main.tile[i, j].type == Mod.TileType("WastelandDirt"))
                        {
                            nextHouseCooldown = WorldGen.genRand.Next(100, 150);
                            WastelandRuins.PlaceHouse(i, j);
                        }
                    }
                }

                nextHouseCooldown--;
            }
        }

        private static void SpreadingGrass(GenerationProgress progress)
        {
            ushort dirtType = (ushort)Mod.TileType("WastelandDirt");
            ushort grassType = (ushort)ModContent.TileType<WastelandGrass>();
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

        private static void GeneratePerlinCavern(GenerationProgress progress)
        {
            int MAX_WASTELAND_HEIGHT = 600;

            progress.Message = "Digging the wasteland for research";

            for (int i = 0; i < 1000; i++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), 500 + WorldGen.genRand.Next(0, 800), (double)WorldGen.genRand.Next(30, 50), 70, Mod.TileType("Wasteland_Dirt"), false);
            }

            for (int i = 0; i < 300; i++)
            {

                WorldGen.TileRunner(WorldGen.genRand.Next(Main.maxTilesX), 500 + WorldGen.genRand.Next(0, 800), WorldGen.genRand.Next(40, 60), 100, -1, true);

            }

            for (int i = 0; i < 500; i++)
            {

                WorldGen.TileRunner(WorldGen.genRand.Next(Main.maxTilesX), 500 + WorldGen.genRand.Next(0, 800), WorldGen.genRand.Next(10, 20), 100, -1, true, WorldGen.genRand.NextFloat(-2f, 2f), WorldGen.genRand.NextFloat(-2f, 2f));
            }

            for (int i = 0; i < 200; i++)
            {
                Vector2 coordinate = new Vector2(WorldGen.genRand.Next(Main.maxTilesX), 500 + WorldGen.genRand.Next(0, 800));
                if (WorldGen.genRand.Next(5) == 0)
                {
                    int strenght = WorldGen.genRand.Next(25, 40);
                    WorldGen.TileRunner((int)coordinate.X, (int)coordinate.Y, strenght, WorldGen.genRand.Next(15, 20), -1, true, WorldGen.genRand.NextFloat(-3f, 3f), WorldGen.genRand.NextFloat(-3f, 3f));
                    GeneralWorldHelper.TileRunner((int)coordinate.X, (int)coordinate.Y, strenght - 5, WorldGen.genRand.Next(15, 20), -5, 0, false, WorldGen.genRand.NextFloat(-3f, 3f), WorldGen.genRand.NextFloat(-3f, 3f));
                    GeneralWorldHelper.TileRunner((int)coordinate.X, (int)coordinate.Y, strenght + 4, WorldGen.genRand.Next(15, 20), ModContent.TileType<WastelandWaste>(), 0, false, WorldGen.genRand.NextFloat(-3f, 3f), WorldGen.genRand.NextFloat(-3f, 3f));
                }
                else
                {
                    WorldGen.TileRunner((int)coordinate.X, (int)coordinate.Y, 20, 1, -1, true, WorldGen.genRand.NextFloat(-3f, 3f), WorldGen.genRand.NextFloat(-3f, 3f));
                }
            }
        }

        private static void GenerateMossCavern(GenerationProgress progress)
        {
            int MAX_WASTELAND_HEIGHT = 600;

            progress.Message = "Creating Moss Cave";

            WastelandLostCave lostCave = new WastelandLostCave(new Rectangle(50, 500, 200, 200));
            lostCave.Generate(progress);
        }

        internal static void TerrainTop(GenerationProgress progress)
        {
            //Create the top of the wasteland
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY - 195; j++)
                {
                    if (WorldGen.genRand.Next(20) == 0)
                    {
                        WorldGen.TileRunner(i, j, (double)WorldGen.genRand.Next(4, 6), 50, Mod.TileType("WastelandRock"), true);
                    }
                }
            }

            //then get rid of the bottom of the world, wasteland rock will need an evil pick to be mined
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type != Mod.TileType("WastelandRock"))
                    {
                        Main.tile[i, j].active(false);
                        Main.tile[i, j].liquid = 0;
                        Main.tile[i, j].wall = 0;
                        Main.tile[i, j].slope(0);
                    }
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
                            PlaceTile(num260, num261, ModContent.TileType<WastelandForge>());
                            if (Main.tile[num260, num261].type == ModContent.TileType<WastelandForge>())
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
