using ROI.Worlds.Underworld;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Worlds
{
    //Contain wasteland world gen
    internal partial class ROIWorld : ModWorld
    {
        internal void WastelandGeneration(GenerationProgress progress)
        {
            
            progress.Message = "Dropping the nuke in the underworld";
            //actual world gen
            if (WorldGen.gen)
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
            }
            TerrainBottom(progress);
            SpreadingGrass(progress);
            GenerateCavern(progress);
            //GrowingTree(progress);
            
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
                        WorldGen.SpreadGrass(i, j, dirtType, grassType, true);
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
                
                ROIWorldHelper.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(MAX_WASTELAND_HEIGHT, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(20, 50), type);
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
                }

                for (int j = wastelandRock; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].active(true);
                    Main.tile[i, j].slope(0);
                    Main.tile[i, j].type = (ushort)mod.TileType("Wasteland_Rock");
                }
            }
        }
    }
}
