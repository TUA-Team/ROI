using System;
using System.Collections.Generic;
using ROI.Worlds.Structures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Worlds
{
    //Contain wasteland world gen
    internal partial class ROIWorld : ModWorld
    {

        internal static Dictionary<int, int> SurfaceLevel;
        internal static int HighestLevel = 300;

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
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY -200; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].active(false);
                }
            }
            float[] freq = new float[] { 0.00077f, 0.00021f, 0.022f };;
            float[] limit = new float[] { 0.3f, 0.05f, 0.02f };
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
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                    WorldGen.TileRunner(i, SurfaceLevel[i] + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(14, 20), WorldGen.genRand.Next(20, 28), -1, true);
            }
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
            GeneratingRuins(progress);
            //GrowingTree(progress);
            //GenerateWasteLake(progress);
            //GenerateMysteriousGrotto(progress);
            WorldGen.EveryTileFrame();
        }

        private void GeneratingRuins(GenerationProgress progress)
        {
            int nextHouseCooldown = 0;
            for (int i = (int) (Main.maxTilesX * 0.30); i < Main.maxTilesX * 0.70; i++)
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
            ushort dirtType = (ushort) mod.TileType("Wasteland_Dirt");
            ushort grassType = (ushort) mod.TileType("Wasteland_Grass");
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
                float percent = (float) (i / Main.maxTilesX * 0.15);
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
                        nextModifierCoolDown = (int) (WorldGen.genRand.Next(20, 50) * 0.5);
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
    }
}
