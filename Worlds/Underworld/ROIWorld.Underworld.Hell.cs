using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using static Terraria.WorldGen;

namespace ROI.Worlds
{
    //Contains refactored hell world gen, for later use
    //may break other mods IL injections?
    internal partial class ROIWorld : ModWorld
    {
        public void OriginalUnderworldGeneration(GenerationProgress progress)
        {
            progress.Message = Language.GetTextValue("LegacyWorldGen.18");
            progress.Set(0f);
            CarveHellTop(progress);
            HellBottom(progress);
            RandomAshCeiling();
            //Settle all the freshly placed lava
            Liquid.QuickWater(-2, -1, -1);
            RandomAshHills(progress);
            LavaPocket(progress);
            //Wtf on that one?
            GuaranteedLavaLine();
            HellstoneOreGeneration(progress);
            progress.Message = "Hell worker house";
            WorldGen.AddHellHouses();
            HellForgeGen(progress);
        }

        /// <summary>
        /// This method make the top section of hell
        /// </summary>
        /// <param name="progress"></param>
        private void CarveHellTop(GenerationProgress progress)
        {
            progress.Message = "Carving the ceiling of hell";
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
                        Main.tile[x, y].type = TileID.Ash;
                    }
                }
            }
        }

        /// <summary>
        /// This method place lava at the bottom of hell
        /// </summary>
        /// <param name="progress"></param>
        private void HellBottom(GenerationProgress progress)
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

        private void RandomAshCeiling()
        {
            for (int currentX = 0; currentX < Main.maxTilesX; currentX++)
            {
                if (WorldGen.genRand.Next(50) == 0)
                {
                    int y = Main.maxTilesY - 65;
                    while (!Main.tile[currentX, y].active() && y > Main.maxTilesY - 135)
                    {
                        y--;
                    }

                    WorldGen.TileRunner(
                        i: WorldGen.genRand.Next(0, Main.maxTilesX),
                        j: y + WorldGen.genRand.Next(20, 50),
                        strength: WorldGen.genRand.Next(15, 20),
                        steps: 1000,
                        type: TileID.Ash,
                        addTile: true,
                        speedX: 0f,
                        speedY: WorldGen.genRand.Next(1, 3),
                        noYChange: true);
                }
            }
        }

        private void RandomAshHills(GenerationProgress progress)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                float num6 = x / (float)(Main.maxTilesX - 1);
                progress.Set(num6 / 2f + 0.5f);
                if (WorldGen.genRand.Next(13) == 0)
                {
                    int num7 = Main.maxTilesY - 65;
                    while ((Main.tile[x, num7].liquid > 0 || Main.tile[x, num7].active()) && num7 > Main.maxTilesY - 140)
                    {
                        num7--;
                    }

                    WorldGen.TileRunner(x, num7 - WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(5, 30), 1000, 57, true, 0f, WorldGen.genRand.Next(1, 3), true);
                    float num8 = WorldGen.genRand.Next(1, 3);
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        num8 *= 0.5f;
                    }

                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        WorldGen.TileRunner(x, num7 - WorldGen.genRand.Next(2, 5), (int)(WorldGen.genRand.Next(5, 15) * num8), (int)(WorldGen.genRand.Next(10, 15) * num8), 57, true, 1f, 0.3f);
                    }

                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        num8 = WorldGen.genRand.Next(1, 3);
                        WorldGen.TileRunner(x, num7 - WorldGen.genRand.Next(2, 5), (int)(WorldGen.genRand.Next(5, 15) * num8), (int)(WorldGen.genRand.Next(10, 15) * num8), 57, true, -1f, 0.3f);
                    }

                    WorldGen.TileRunner(x + WorldGen.genRand.Next(-10, 10), num7 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        WorldGen.TileRunner(x + WorldGen.genRand.Next(-10, 10), num7 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
                    }

                    if (WorldGen.genRand.Next(5) == 0)
                    {
                        WorldGen.TileRunner(x + WorldGen.genRand.Next(-15, 15), num7 + WorldGen.genRand.Next(-15, 10), WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
                    }
                }
            }
        }

        private void LavaPocket(GenerationProgress progress)
        {
            progress.Message = "Creating lava pockets";
            for (int num9 = 0; num9 < Main.maxTilesX; num9++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2);
            }
        }

        private void GuaranteedLavaLine()
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

        private void HellstoneOreGeneration(GenerationProgress progress)
        {
            progress.Message = Language.GetTextValue("LegacyWorldGen.18");
            for (int num11 = 0; num11 < (int)(Main.maxTilesX * Main.maxTilesY * 0.0008); num11++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), 58);
            }
        }

        private static void HellForgeGen(GenerationProgress progress)
        {
            progress.Message = Lang.gen[36].Value;
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
                        if (Main.tile[num260, num261].wall == 13 || Main.tile[num260, num261].wall == 14)
                        {
                            for (; !Main.tile[num260, num261].active(); num261++)
                            {
                            }

                            num261--;
                            PlaceTile(num260, num261, 77);
                            if (Main.tile[num260, num261].type == 77)
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