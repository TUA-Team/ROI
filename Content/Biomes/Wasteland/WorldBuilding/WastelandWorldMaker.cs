// TODO: wasteland worldgen

/*using Microsoft.Xna.Framework;
using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
//using ROI.Content.Biomes.Wasteland.WorldBuilding.Vines;
using ROI.Utilities;
using ROI.Utilities.Models;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.WorldGen;

// TODO: (low prio) torches
namespace ROI.Content.Biomes.Wasteland.WorldBuilding
{
    //Contain wasteland world gen
    public class WastelandWorldMaker
    {
        *//*private readonly Dictionary<int, int> SurfaceLevel = new Dictionary<int, int>();
        private int HighestLevel = 300;*//*
        private Mod Mod { get; }

        private readonly ushort UraniumOreType;
        private readonly ushort OreType;
        private readonly ushort DirtType;
        private readonly ushort RockType;
        private readonly ushort GrassType;

        public WastelandWorldMaker(Mod mod)
        {
            Mod = mod;

            UraniumOreType = (ushort)mod.TileType(nameof(UraniumOre));
            OreType = (ushort)mod.TileType(nameof(WastelandOre));
            DirtType = (ushort)mod.TileType(nameof(WastelandDirt));
            RockType = (ushort)mod.TileType(nameof(WastelandRock));
            GrassType = (ushort)mod.TileType(nameof(WastelandGrass));
        }

        public void Make(GenerationProgress progress)
        {
            // TODO: (low prio) localization?
            progress.Message = "Accumulating waste";

            int count = Main.maxTilesX / 2100;

            for (int i = 0; i < count; i++)
            {
                bool flag = true;
                var pos = getLocation();

                while (flag)
                {
                    var tile = Main.tile[pos.X, pos.Y];
                    if (!tile.HasTile &&
                        tile.type != TileID.Granite &&
                        tile.wall != WallID.GraniteUnsafe)
                    {

                        goto GEN;
                    }

                    pos = getLocation();
                }

            GEN:
                GenerateBiome(pos);
            }


            Point getLocation()
            {
                return new Point(genRand.Next((int)(Main.maxTilesX * 0.25), (int)(Main.maxTilesX * 0.75)),
                    genRand.Next((int)Main.rockLayer + 100, Main.maxTilesY - 300));
            }


            return;
*//*
            // Bottom
            BaseTerrain(new float[] { 0.0077f, 0.0011f, 0.022f, 0.04f }, new float[] { 0.7f, 0.05f, 0.02f, 0.01f }, new int[] { 5, 5, 5, 5 }, 450);
            // Top
            BaseTerrain(new float[] { 0.07f, 0.04f, 0.03f }, new float[] { 0.7f, 0.04f, 0.06f }, new int[] { 5, 5, 5, 5 }, 200, true);
            progress.Message = "Accumulating waste";
            // actual world gen
            //StructureMap map = new StructureMap();

            SpreadingGrass(progress);
            GeneratePerlinCavern(progress);
            GenerateMossCavern(progress);
            GenerateWasteLake(progress);
            WastelandOreGeneration(progress);
            //GeneratingRuins(progress);
            //WastelandForgeGen(progress);
            WastelandUraniumOreGen(progress);
            //GrowingTree(progress);
            //GenerateMysteriousGrotto(progress);*//*
        }


        private void GenerateBiome(Point center)
        {
            Point pos = new Point(center.X - 100, center.Y - 70);
            int width = 200;
            int height = 140;

            // Generate dirt with some anchor points
            for (int i = 0; i < 3; i++)
            {
                var angle = MathHelper.ToRadians(genRand.Next(360));
                int dist = genRand.Next(20, 30);
                int x = (int)(Math.Sin(angle) * dist) + center.X;
                int y = (int)(Math.Cos(angle) * dist) + center.Y;

                TileRunner(x, y, genRand.Next(50, 120), 30, DirtType, true, genRand.Next(7, 10), genRand.Next(-2, 2));
                TileRunner(x, y, genRand.Next(50, 120), 30, DirtType, true, genRand.Next(-10, -7), genRand.Next(-2, 2));
            }

            // Generate holes
            var noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            noise.SetFractalOctaves(5);
            noise.SetFrequency(0.06f);

            for (int x = pos.X; x < pos.X + width; x++)
            {
                for (int y = pos.Y; y < pos.Y + height; y++)
                {
                    float rand = noise.GetNoise(x, y);
                    if (rand < -0.1f)
                    {
                        var tile = Main.tile[x, y];
                        if (tile.type == DirtType)
                        {
                            tile.active(false);
                        }
                    }
                }
            }

            // Add rocks and grass and vines
            for (int x = pos.X; x < pos.X + width; x++)
            {
                for (int y = pos.Y; y < pos.Y + height; y++)
                {
                    if (TileUtils.IsExposed(x, y) &&
                        Main.tile[x, y].type == DirtType)
                    {
                        Main.tile[x, y].type = GrassType;

                        if (!Main.tile[x, y + 1].HasTile)
                        {
                            // Baseline is 3, use noise to go smaller or bigger
                            int len = (int)(3 + noise.GetNoise(x, y) * 4) + genRand.Next(6);
                            if (len > 0)
                            {
                                int index = ModContent.GetInstance<TEWastelandVine>().Place(x, y);
                                TEWastelandVine te = TileEntity.ByID[index] as TEWastelandVine;
                                te.GenerateDrawData(len);
                                te.GeneratePoints();
                                te.GenerateSegments();
                            }
                        }
                    }

                    else if (genRand.Next(1400) == 0)
                    {
                        TileRunner(x, y, genRand.Next(15, 20), genRand.Next(2, 5), RockType, false, genRand.Next(-3, 3), genRand.Next(-6, 6));
                    }
                }
            }

            TileUtils.TrimTileRunnerAftermath(pos.X, pos.Y, width, height);
        }


*//*        private int GetAmountOfBigLakes()
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

        private int[] GetPerlinDisplacements(int displacementCount, float frequency, int maxLimit, float multiplier, int octave, int seed)
        {
            FastNoiseLite noise = new FastNoiseLite(seed);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            noise.SetFrequency(frequency);
            noise.SetFractalOctaves(octave);

            int[] displacements = new int[displacementCount];

            for (int x = 0; x < displacementCount; x++)
                displacements[x] = (int)Math.Floor(noise.GetNoise(x, x) * maxLimit * multiplier);
            return displacements;
        }

        private int[,] GetPerlinDisplacements2D(int width, int height, float frequency, int maxLimit, float multiplier, int octave, int seed)
        {
            FastNoiseLite noise = new FastNoiseLite(seed);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
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

        private FastNoiseLite GeneratePerlin(float gain, float lucanarity, float frequency, int octave, int seed)
        {
            FastNoiseLite noise = new FastNoiseLite(seed);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            noise.SetFrequency(frequency);
            noise.SetFractalOctaves(octave);

            return noise;
        }

        private void WastelandUraniumOreGen(GenerationProgress progress)
        {
            for (int i = 0; i < 50; i++)
            {
                int x = Main.rand.Next(25, Main.maxTilesX - 25);
                if (Scan(x, 300, out int yGen, tile => true))
                {
                    WorldUtils.Gen(new Point(x, yGen), new Shapes.Tail(WorldGen.genRand.Next(10, 13), new Vector2(WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(20, 25))), Actions.Chain(new GenAction[]
                    {
                        new Actions.PlaceTile(UraniumOreType, 0)
                    }));
                }
            }
        }

        private void BaseTerrain(float[] freq, float[] limit, int[] octave, int startingHeight, bool top = false)
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
                    TileHelper.Fill(i, totalDisplacements[i], 1, dirtDepth, DirtType);
                    TileHelper.Fill(i, totalDisplacements[i] + dirtDepth, 1, 900, RockType);
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
                    TileHelper.FillInvertYAxis(i, totalDisplacements[i], 1, 400, RockType);
                }
            }


            for (int i = 0; i < Main.maxTilesX; i++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                    WorldGen.TileRunner(i, SurfaceLevel[i] + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(14, 20), WorldGen.genRand.Next(20, 28), -1, true);
            }
        }

        private void WastelandOreGeneration(GenerationProgress progress)
        {
            for (int num11 = 0; num11 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num11++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(400, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), OreType, false, 0f, 0f, false, true);
            }
        }

        private void GenerateWasteLake(GenerationProgress progress)
        {
            int smallLake = 0;
            int bigLake = 0;

            for (int i = 0; i < 25; i++)
            {
                int x = Main.rand.Next(200, Main.maxTilesX - 200);
                if (Scan(x, 200, out int yGen, tile => !(tile.type != DirtType && tile.liquid > 0)))
                {
                    if (bigLake != GetAmountOfBigLakes())
                    {
                        //WastelandLake.Generate(x, yGen, 40, 55, 8f, 3, 3, false);
                        bigLake++;
                        continue;
                    }

                    //                    WastelandLake.Generate(x, yGen, 20, 24, 5f, 3, 3, true);
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

                if (!Main.tile[x, i].HasTile && Main.tile[x, i].liquid == 0)
                {
                    continue;
                }
                else
                {
                    if (condition?.Invoke(Main.tile[x, i]) ?? false)
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
                    if (Scan(i, 250, out int j, tile => !(tile.type != DirtType && tile.liquid > 0)))
                    {
                        if (Main.tile[i, j].type == DirtType)
                        {
                            nextHouseCooldown = WorldGen.genRand.Next(100, 150);
                            WastelandRuins.PlaceHouse(i, j);
                        }
                    }
                }

                nextHouseCooldown--;
            }
        }

        private void SpreadingGrass(GenerationProgress progress)
        {
            ushort grassType = (ushort)ModContent.TileType<WastelandGrass>();
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                float percent = (float)(i / Main.maxTilesX);
                progress.Set(percent);
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type == DirtType && !Main.tile[i, j - 1].HasTile)
                    {
                        Main.tile[i, j].wall = 0;
                        SpreadGrass(i, j, DirtType, grassType, true);
                    }
                }
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                float percent = i / Main.maxTilesX;
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

        private void GeneratePerlinCavern(GenerationProgress progress)
        {
            int MAX_WASTELAND_HEIGHT = 600;

            progress.Message = "Digging the wasteland for research";

            for (int i = 0; i < 1000; i++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), 500 + WorldGen.genRand.Next(0, 800), (double)WorldGen.genRand.Next(30, 50), 70, DirtType, false);
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
                    //GeneralWorldHelper.TileRunner((int)coordinate.X, (int)coordinate.Y, strenght - 5, WorldGen.genRand.Next(15, 20), -5, 0, false, WorldGen.genRand.NextFloat(-3f, 3f), WorldGen.genRand.NextFloat(-3f, 3f));
                    //GeneralWorldHelper.TileRunner((int)coordinate.X, (int)coordinate.Y, strenght + 4, WorldGen.genRand.Next(15, 20), ModContent.TileType<WastelandWaste>(), 0, false, WorldGen.genRand.NextFloat(-3f, 3f), WorldGen.genRand.NextFloat(-3f, 3f));
                }
                else
                {
                    WorldGen.TileRunner((int)coordinate.X, (int)coordinate.Y, 20, 1, -1, true, WorldGen.genRand.NextFloat(-3f, 3f), WorldGen.genRand.NextFloat(-3f, 3f));
                }
            }
        }

        private void GenerateMossCavern(GenerationProgress progress)
        {
            int MAX_WASTELAND_HEIGHT = 600;

            progress.Message = "Creating Moss Cave";

            //WastelandLostCave lostCave = new WastelandLostCave(new Rectangle(50, 500, 200, 200));
            //lostCave.Generate(progress);
        }

        public void TerrainTop(GenerationProgress progress)
        {
            //Create the top of the wasteland
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY - 195; j++)
                {
                    if (WorldGen.genRand.Next(20) == 0)
                    {
                        WorldGen.TileRunner(i, j, (double)WorldGen.genRand.Next(4, 6), 50, RockType, true);
                    }
                }
            }

            //then get rid of the bottom of the world, wasteland rock will need an evil pick to be mined
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type != RockType)
                    {
                        Main.tile[i, j].active(false);
                        Main.tile[i, j].liquid = 0;
                        Main.tile[i, j].wall = 0;
                        Main.tile[i, j].slope(0);
                    }
                }
            }
        }

        private void WastelandForgeGen(GenerationProgress progress)
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
                    if (Main.tile[num260, num261].wall == Mod.WallType(nameof(WastestoneBrickWall)))
                    {
                        *//*for (; !Main.tile[num260, num261].HasTile; num261++)
                        {
                        }*//*

                        num261--;
                        PlaceTile(num260, num261, Mod.TileType(nameof(WastelandForge)));
                        if (Main.tile[num260, num261].type == Mod.TileType(nameof(WastelandForge)))
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
            }
        }*//*
    }
}
*/