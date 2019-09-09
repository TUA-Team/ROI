using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Worlds
{
    //Contain refactored hell world gen, for later use
    partial class ROIWorld : ModWorld
    {
        internal void OriginalUnderworldGeneration(GenerationProgress progress)
        {
            progress.Message = Language.GetTextValue("LegacyWorldGen.18");
            progress.Set(0f);
            CarveHellTop(progress);
            HellBottom(progress);
            ToDetermineLater(progress);
            //Settle all the freshly placed lava
            Liquid.QuickWater(-2, -1, -1);
            ToDetermineLater2(progress);
            LavaPocket(progress);
            //Wtf on that one?
            GuaranteedLavaLine();
            HellstoneOreGeneration(progress);
            progress.Message = "Hell worker house";
            WorldGen.AddHellHouses();
        }

        private static void HellstoneOreGeneration(GenerationProgress progress)
        {
            progress.Message = Lang.gen[18].Value;
            for (int num11 = 0; num11 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num11++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), 58, false, 0f, 0f, false, true);
            }
        }

        private static void GuaranteedLavaLine()
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                if (!Main.tile[x, Main.maxTilesY - 145].active())
                {
                    Main.tile[x, Main.maxTilesY - 145].liquid = 255;
                    Main.tile[x, Main.maxTilesY - 145].lava(true);
                }

                if (!Main.tile[x, Main.maxTilesY - 144].active())
                {
                    Main.tile[x, Main.maxTilesY - 144].liquid = 255;
                    Main.tile[x, Main.maxTilesY - 144].lava(true);
                }
            }
        }

        private static void LavaPocket(GenerationProgress progress)
        {
            progress.Message = "Placing lava pocket";
            for (int num9 = 0; num9 < Main.maxTilesX; num9++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2, false, 0f, 0f, false, true);
            }
        }

        private static void ToDetermineLater2(GenerationProgress progress)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                float num6 = (float)x / (float)(Main.maxTilesX - 1);
                progress.Set(num6 / 2f + 0.5f);
                if (WorldGen.genRand.Next(13) == 0)
                {
                    int num7 = Main.maxTilesY - 65;
                    while ((Main.tile[x, num7].liquid > 0 || Main.tile[x, num7].active()) && num7 > Main.maxTilesY - 140)
                    {
                        num7--;
                    }

                    WorldGen.TileRunner(x, num7 - WorldGen.genRand.Next(2, 5), (double)WorldGen.genRand.Next(5, 30), 1000, 57, true, 0f, (float)WorldGen.genRand.Next(1, 3), true, true);
                    float num8 = (float)WorldGen.genRand.Next(1, 3);
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        num8 *= 0.5f;
                    }

                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        WorldGen.TileRunner(x, num7 - WorldGen.genRand.Next(2, 5), (double)((int)((float)WorldGen.genRand.Next(5, 15) * num8)), (int)((float)WorldGen.genRand.Next(10, 15) * num8), 57, true, 1f, 0.3f, false, true);
                    }

                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        num8 = (float)WorldGen.genRand.Next(1, 3);
                        WorldGen.TileRunner(x, num7 - WorldGen.genRand.Next(2, 5), (double)((int)((float)WorldGen.genRand.Next(5, 15) * num8)), (int)((float)WorldGen.genRand.Next(10, 15) * num8), 57, true, -1f, 0.3f, false, true);
                    }

                    WorldGen.TileRunner(x + WorldGen.genRand.Next(-10, 10), num7 + WorldGen.genRand.Next(-10, 10), (double)WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, false, (float)WorldGen.genRand.Next(-1, 3), (float)WorldGen.genRand.Next(-1, 3), false, true);
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        WorldGen.TileRunner(x + WorldGen.genRand.Next(-10, 10), num7 + WorldGen.genRand.Next(-10, 10), (double)WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, false, (float)WorldGen.genRand.Next(-1, 3), (float)WorldGen.genRand.Next(-1, 3), false, true);
                    }

                    if (WorldGen.genRand.Next(5) == 0)
                    {
                        WorldGen.TileRunner(x + WorldGen.genRand.Next(-15, 15), num7 + WorldGen.genRand.Next(-15, 10), (double)WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, false, (float)WorldGen.genRand.Next(-1, 3), (float)WorldGen.genRand.Next(-1, 3), false, true);
                    }
                }
            }
        }


        private static void ToDetermineLater(GenerationProgress progress)
        {
            for (int num3 = 0; num3 < Main.maxTilesX; num3++)
            {
                if (WorldGen.genRand.Next(50) == 0)
                {
                    int num4 = Main.maxTilesY - 65;
                    while (!Main.tile[num3, num4].active() && num4 > Main.maxTilesY - 135)
                    {
                        num4--;
                    }

                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), num4 + WorldGen.genRand.Next(20, 50), (double)WorldGen.genRand.Next(15, 20), 1000, 57, true, 0f, (float)WorldGen.genRand.Next(1, 3), true, true);
                }
            }
        }

        /// <summary>
        /// This method place lava at the bottom of hell
        /// </summary>
        /// <param name="progress"></param>
        private static void HellBottom(GenerationProgress progress)
        {
            progress.Message = "Creating the lava pit";
            progress.Set(0.2f);
            int currentY = Main.maxTilesY - WorldGen.genRand.Next(40, 70);
            for (int x = 10; x < Main.maxTilesX - 10; x++)
            {
                currentY += WorldGen.genRand.Next(-10, 11);
                if (currentY > Main.maxTilesY - 60)
                {
                    currentY = Main.maxTilesY - 60;
                }

                if (currentY < Main.maxTilesY - 100)
                {
                    currentY = Main.maxTilesY - 120;
                }

                for (int y = currentY; y < Main.maxTilesY - 10; y++)
                {
                    if (!Main.tile[x, y].active())
                    {
                        Main.tile[x, y].lava(true);
                        Main.tile[x, y].liquid = 255;
                    }
                }
            }
        }

        /// <summary>
        /// This method make the top section of hell
        /// </summary>
        /// <param name="progress"></param>
        private static void CarveHellTop(GenerationProgress progress)
        {
            progress.Message = "Carving the entry of hell";
            progress.Set(0.1f);
            int currentY = Main.maxTilesY - WorldGen.genRand.Next(150, 190);
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                currentY += WorldGen.genRand.Next(-3, 4);
                if (currentY < Main.maxTilesY - 190)
                {
                    currentY = Main.maxTilesY - 190;
                }

                if (currentY > Main.maxTilesY - 160)
                {
                    currentY = Main.maxTilesY - 160;
                }

                for (int y = currentY - 20 - WorldGen.genRand.Next(3); y < Main.maxTilesY; y++)
                {
                    if (y >= currentY)
                    {
                        Main.tile[x, y].active(false);
                        Main.tile[x, y].lava(false);
                        Main.tile[x, y].liquid = 0;
                    }
                    else
                    {
                        Main.tile[x, y].type = 57;
                    }
                }
            }
        }
    }
}
