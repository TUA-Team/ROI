using ROI.Tiles.Wasteland;
using ROI.Walls.Wasteland;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Worlds
{
    internal sealed partial class ROIWorld : ModWorld
    {
        public void WastelandGeneration(GenerationProgress progress)
        {
            progress.Message = "Spreading radioactivity";
            //actual world gen
            if (WorldGen.gen)
            {
                for (int i = 0; i < 10; i++) TerrainTop();
            }
            //Debug stick gen
            else
            {
                TerrainTop();
            }
            TerrainBottom();
            SpreadGrass(progress);
            GenerateCaves(progress);
            GenerateRuins();
            //GeneratingTheLab(progress);
            //GrowingTree(progress);
            WorldGen.EveryTileFrame();
        }

        private void GenerateRuins()
        {
            int cooldown = 0;
            for (int i = (int)(Main.maxTilesX * 0.30); i < Main.maxTilesX * 0.70; i++)
            {
                if (cooldown <= 0 && WorldGen.genRand.Next(5) == 0)
                {
                    for (int j = Main.maxTilesY - 120; j < Main.maxTilesY - 20; j++)
                    {
                        if (Main.tile[i, j].type == ModContent.TileType<WastelandDirt>())
                        {
                            cooldown = WorldGen.genRand.Next(100, 150);
                            Structures.WastelandRuins.PlaceHouse(i, j);
                            break;
                        }
                    }
                }
                cooldown--;
            }
        }

        private void SpreadGrass(GenerationProgress progress)
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                progress.Set(i / Main.maxTilesX);
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type == ModContent.TileType<WastelandDirt>() && !Main.tile[i, j - 1].active())
                    {
                        Main.tile[i, j].wall = 0;
                        WorldGen.SpreadGrass(i, j, ModContent.TileType<WastelandDirt>(), ModContent.TileType<WastelandGrass>(), true);
                    }
                }
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                progress.Set(i / Main.maxTilesX);
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type == ModContent.TileType<WastelandGrass>()) Main.tile[i, j].wall = 0;
                }
            }
        }

        private void GenerateCaves(GenerationProgress progress)
        {
            for (int i = 0; i < Main.maxTilesX * 0.15; i++)
            {
                progress.Set((float)(i / Main.maxTilesX * 0.15));

                ROIWorldHelper.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
                    WorldGen.genRand.Next(Main.maxTilesY - 130, Main.maxTilesY),
                    WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(50, 70), -1);
                ROIWorldHelper.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
                    WorldGen.genRand.Next(Main.maxTilesY - 130, Main.maxTilesY),
                    WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(20, 50), ModContent.TileType<WastelandDirt>());
            }
        }

        public void TerrainTop()
        {
            //Create the top of the wasteland
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY - 195; j++)
                {
                    if (WorldGen.genRand.Next(20) == 0)
                    {
                        WorldGen.TileRunner(i, j, WorldGen.genRand.Next(4, 6), 50, ModContent.TileType<WastelandRock>(), true);
                    }
                }
            }

            //then get rid of the bottom of the world, wasteland (ushort)mod.TileType<WastelandRock>() will need an evil pick to be mined
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].type != (ushort)ModContent.TileType<WastelandRock>())
                    {
                        Main.tile[i, j].active(false);
                        Main.tile[i, j].liquid = 0;
                        Main.tile[i, j].wall = 0;
                        Main.tile[i, j].slope(0);
                    }
                }
            }
        }

        public void TerrainBottom()
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
                    Main.tile[i, j].type = (ushort)ModContent.TileType<WastelandDirt>();
                    Main.tile[i, j].wall = (ushort)ModContent.WallType<WastelandDirtWall>();
                }

                for (int j = wastelandRock; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].active(true);
                    Main.tile[i, j].slope(0);
                    Main.tile[i, j].type = (ushort)ModContent.TileType<WastelandRock>();
                    Main.tile[i, j].wall = (ushort)ModContent.WallType<WastelandRockWall>();
                }
            }
        }
    }
}