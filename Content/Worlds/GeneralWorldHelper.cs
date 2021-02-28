using System;
using Terraria;
using Terraria.ID;

namespace ROI.Worlds
{
    public static class GeneralWorldHelper
    {
        public static bool IsExposed(int x, int y) => CountNeighbors(x, y) <= 3;

        public static int CountNeighbors(int x, int y, Func<int, int, bool> isValid = null)
        {
            int count = 0;
            if (checkValid(x + 1, y))
            {
                count += 1;
            }
            if (checkValid(x - 1, y))
            {
                count += 1;
            }
            if (checkValid(x, y + 1))
            {
                count += 1;
            }
            if (checkValid(x, y - 2))
            {
                count += 1;
            }

            return count;

            bool checkValid(int i, int j)
            {
                return Main.tile[i, j].active() && (isValid == null || isValid(i, j));
            }
        }

        public static void TrimTileRunnerAftermath(int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (!IsTileTouchingDirectly(i, j))
                    {
                        Main.tile[i, j].active(false);
                    }
                }
            }

            bool IsTileTouchingDirectly(int i, int j)
            {
                return Main.tile[i + 1, j].active() || Main.tile[i - 1, j].active() || Main.tile[i, j + 1].active() || Main.tile[i, j - 1].active();
            }
        }

        #region Obsolete liquid utils

        /*public static void FillLiquid(int i, int j, int width, int height, ModLiquid liquidID, bool stopIfTileHit = false)
        {
            for (int x = i; x < i + width; x++)
            {
                for (int y = j; y < j + height; y++)
                {
                    if (!WorldGen.InWorld(x, y))
                    {
                        break;
                    }
                    if (Main.tile[i, j].active() && stopIfTileHit)
                    {
                        continue;
                    }
                    else
                    {
                        LiquidRef liquidRef = LiquidWorld.grid[x, y];
                        liquidRef.Amount = 255;
                        liquidRef.LiquidType = liquidID;
                    }
                }
            }
        }*/

        /*public static void TileRunner(int i, int j, double strength, int steps, int type, int wallType = 0, bool addTile = false, float speedX = 0f, float speedY = 0f, bool noYChange = false, bool overRide = true)
        {
            double num = strength;
            float num2 = (float)steps;
            Vector2 value;
            value.X = (float)i;
            value.Y = (float)j;
            Vector2 value2;
            value2.X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
            value2.Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
            if (speedX != 0f || speedY != 0f)
            {
                value2.X = speedX;
                value2.Y = speedY;
            }
            bool flag = type == 368;
            bool flag2 = type == 367;
            while (num > 0.0 && num2 > 0f)
            {
                if (value.Y < 0f && num2 > 0f && type == 59)
                {
                    num2 = 0f;
                }
                num = strength * (double)(num2 / (float)steps);
                num2 -= 1f;
                int num3 = (int)((double)value.X - num * 0.5);
                int num4 = (int)((double)value.X + num * 0.5);
                int num5 = (int)((double)value.Y - num * 0.5);
                int num6 = (int)((double)value.Y + num * 0.5);
                if (num3 < 1)
                {
                    num3 = 1;
                }
                if (num4 > Main.maxTilesX - 1)
                {
                    num4 = Main.maxTilesX - 1;
                }
                if (num5 < 1)
                {
                    num5 = 1;
                }
                if (num6 > Main.maxTilesY - 1)
                {
                    num6 = Main.maxTilesY - 1;
                }
                for (int k = num3; k < num4; k++)
                {
                    for (int l = num5; l < num6; l++)
                    {
                        if ((double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < strength * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
                        {
                            if (type < 0)
                            {
                                // TODO: this should not be in this method
                                switch (type)
                                {
                                    //Lava
                                    case -2:
                                        if (Main.tile[k, l].active() && (l < WorldGen.waterLine || l > WorldGen.lavaLine))
                                        {
                                            Main.tile[k, l].liquid = 255;
                                            if (l > WorldGen.lavaLine)
                                            {
                                                Main.tile[k, l].lava(true);
                                            }
                                        }
                                        break;
                                    //Water
                                    case -3:
                                        if (Main.tile[k, l].active() && (l > WorldGen.waterLine))
                                        {
                                            Main.tile[k, l].liquid = 255;
                                            if (l > WorldGen.waterLine)
                                            {
                                                Main.tile[k, l].liquidType(0);
                                            }
                                        }
                                        break;
                                    //Honey
                                    case -4:
                                        if (Main.tile[k, l].active() && (l > WorldGen.waterLine))
                                        {
                                            Main.tile[k, l].liquid = 255;
                                            if (l > WorldGen.waterLine)
                                            {
                                                Main.tile[k, l].honey(true);
                                            }
                                        }
                                        break;
                                    //Radioactive Waste
                                    case -5:
                                        LiquidRef liqRef = LiquidWorld.grid[k, l];
                                        if (!Main.tile[k, l].active())
                                        {
                                            liqRef.Amount = 255;
                                            liqRef.LiquidType = LiquidRegistry.GetLiquid(ROIMod.Instance, "PlutonicWaste");
                                        }
                                        break;
                                    //Processed radioactive goo
                                    case -6:
                                        //TODO: Introduce stable liquid goo
                                        break;
                                    case -7:
                                        //TODO: Introduce cursed lava
                                        break;
                                    case -8:
                                        //TODO: Introduce ichor lava
                                        break;
                                }

                                if (wallType != 0)
                                {
                                    Main.tile[k, l].wall = (ushort)wallType;
                                }
                                Main.tile[k, l].active(false);
                            }
                            else
                            {
                                if (flag && (double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < strength * 0.3 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.01))
                                {
                                    WorldGen.PlaceWall(k, l, 180, true);
                                }
                                if (flag2 && (double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < strength * 0.3 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.01))
                                {
                                    WorldGen.PlaceWall(k, l, 178, true);
                                }
                                if (overRide || !Main.tile[k, l].active())
                                {
                                    Tile tile = Main.tile[k, l];
                                    bool flag3 = Main.tileStone[type] && tile.type != 1;
                                    if (!TileID.Sets.CanBeClearedDuringGeneration[(int)tile.type])
                                    {
                                        flag3 = true;
                                    }
                                    ushort type2 = tile.type;
                                    if (type2 <= 147)
                                    {
                                        if (type2 <= 45)
                                        {
                                            if (type2 != 1)
                                            {
                                                if (type2 == 45)
                                                {
                                                    break;
                                                }
                                            }
                                            else if (type == 59 && (double)l < Main.worldSurface + (double)WorldGen.genRand.Next(-50, 50))
                                            {
                                                flag3 = true;
                                            }
                                        }
                                        else if (type2 != 53)
                                        {
                                            if (type2 == 147)
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (type == 40)
                                            {
                                                flag3 = true;
                                            }
                                            if ((double)l < Main.worldSurface && type != 59)
                                            {
                                                flag3 = true;
                                            }
                                        }
                                    }
                                    else if (type2 <= 196)
                                    {
                                        switch (type2)
                                        {
                                            case 189:
                                            case 190:
                                                break;
                                            default:
                                                if (type2 == 196)
                                                {
                                                    break;
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        switch (type2)
                                        {
                                            case 367:
                                            case 368:
                                                if (type == 59)
                                                {
                                                    flag3 = true;
                                                }
                                                break;
                                            default:
                                                switch (type2)
                                                {
                                                    case 396:
                                                    case 397:
                                                        flag3 = !TileID.Sets.Ore[type];
                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                IL_5B7:
                                    if (!flag3)
                                    {
                                        tile.type = (ushort)type;
                                        tile.wall = (ushort)wallType;
                                        break;
                                    }
                                    flag3 = true;
                                    goto IL_5B7;
                                }
                                if (addTile)
                                {
                                    Main.tile[k, l].active(true);
                                    Main.tile[k, l].liquid = 0;
                                    Main.tile[k, l].lava(false);
                                }
                                if (noYChange && (double)l < Main.worldSurface && type != 59)
                                {
                                    Main.tile[k, l].wall = 2;
                                }
                                if (type == 59 && l > WorldGen.waterLine && Main.tile[k, l].liquid > 0)
                                {
                                    Main.tile[k, l].lava(false);
                                    Main.tile[k, l].liquid = 0;
                                }
                            }
                        }
                    }
                }
                value += value2;
                if (num > 50.0)
                {
                    value += value2;
                    num2 -= 1f;
                    value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                    value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                    if (num > 100.0)
                    {
                        value += value2;
                        num2 -= 1f;
                        value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                        value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                        if (num > 150.0)
                        {
                            value += value2;
                            num2 -= 1f;
                            value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                            value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                            if (num > 200.0)
                            {
                                value += value2;
                                num2 -= 1f;
                                value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                if (num > 250.0)
                                {
                                    value += value2;
                                    num2 -= 1f;
                                    value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                    value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                    if (num > 300.0)
                                    {
                                        value += value2;
                                        num2 -= 1f;
                                        value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                        value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                        if (num > 400.0)
                                        {
                                            value += value2;
                                            num2 -= 1f;
                                            value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                            value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                            if (num > 500.0)
                                            {
                                                value += value2;
                                                num2 -= 1f;
                                                value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                if (num > 600.0)
                                                {
                                                    value += value2;
                                                    num2 -= 1f;
                                                    value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                    value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                    if (num > 700.0)
                                                    {
                                                        value += value2;
                                                        num2 -= 1f;
                                                        value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                        value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                        if (num > 800.0)
                                                        {
                                                            value += value2;
                                                            num2 -= 1f;
                                                            value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                            value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                            if (num > 900.0)
                                                            {
                                                                value += value2;
                                                                num2 -= 1f;
                                                                value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                                value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                if (value2.X > 1f)
                {
                    value2.X = 1f;
                }
                if (value2.X < -1f)
                {
                    value2.X = -1f;
                }
                if (!noYChange)
                {
                    value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                    if (value2.Y > 1f)
                    {
                        value2.Y = 1f;
                    }
                    if (value2.Y < -1f)
                    {
                        value2.Y = -1f;
                    }
                }
                else if (type != 59 && num < 3.0)
                {
                    if (value2.Y > 1f)
                    {
                        value2.Y = 1f;
                    }
                    if (value2.Y < -1f)
                    {
                        value2.Y = -1f;
                    }
                }
                if (type == 59 && !noYChange)
                {
                    if ((double)value2.Y > 0.5)
                    {
                        value2.Y = 0.5f;
                    }
                    if ((double)value2.Y < -0.5)
                    {
                        value2.Y = -0.5f;
                    }
                    if ((double)value.Y < Main.rockLayer + 100.0)
                    {
                        value2.Y = 1f;
                    }
                    if (value.Y > (float)(Main.maxTilesY - 300))
                    {
                        value2.Y = -1f;
                    }
                }
            }
        }*/

        #endregion

        public static void SpecialTileMerge(int i, int j, int tileToSearch)
        {
            int current = Main.tile[i, j].type;
            int up = Main.tile[i, j - 1].type;
            int down = Main.tile[i, j + 1].type;
            int left = Main.tile[i - 1, j].type;
            int right = Main.tile[i + 1, j].type;
            int upLeft = Main.tile[i - 1, j + 1].type;
            int upRight = Main.tile[i + 1, j + 1].type;
            int downLeft = Main.tile[i - 1, j - 1].type;
            int downRight = Main.tile[i + 1, j - 1].type;

            int current63 = WorldGen.genRand.Next(0, 3);

            Tile tile = Main.tile[i, j];

            WorldGen.TileMergeAttempt(tile.type, Main.tileMerge[current], ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);


            if (tile.frameX != -1 && tile.frameY != -1 && (current > -1 && TileID.Sets.ChecksForMerge[current]))
            {
                if (up != -1 && down != -1 && left != -1 && right != -1)
                {
                    if (up == tileToSearch && down == current && left == current && right == current)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 144;
                            tile.frameY = 108;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 162;
                            tile.frameY = 108;
                        }
                        else
                        {
                            tile.frameX = 180;
                            tile.frameY = 108;
                        }

                        ////WorldGen.mergeUp = true;
                    }
                    else if (up == current && down == tileToSearch && left == current && right == current)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 144;
                            tile.frameY = 90;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 162;
                            tile.frameY = 90;
                        }
                        else
                        {
                            tile.frameX = 180;
                            tile.frameY = 90;
                        }

                        ////WorldGen.mergeDown = true;
                    }
                    else if (up == current && down == current && left == tileToSearch && right == current)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 162;
                            tile.frameY = 126;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 162;
                            tile.frameY = 144;
                        }
                        else
                        {
                            tile.frameX = 162;
                            tile.frameY = 162;
                        }

                        ////WorldGen.mergeLeft = true;
                    }
                    else if (up == current && down == current && left == current && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 144;
                            tile.frameY = 126;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 144;
                            tile.frameY = 144;
                        }
                        else
                        {
                            tile.frameX = 144;
                            tile.frameY = 162;
                        }

                        ////WorldGen.mergeRight = true;
                    }
                    else if (up == tileToSearch && down == current && left == tileToSearch && right == current)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 36;
                            tile.frameY = 90;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 36;
                            tile.frameY = 126;
                        }
                        else
                        {
                            tile.frameX = 36;
                            tile.frameY = 162;
                        }

                        ////WorldGen.mergeUp = true;
                        ////WorldGen.mergeLeft = true;
                    }
                    else if (up == tileToSearch && down == current && left == current && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 54;
                            tile.frameY = 90;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 54;
                            tile.frameY = 126;
                        }
                        else
                        {
                            tile.frameX = 54;
                            tile.frameY = 162;
                        }

                        ////WorldGen.mergeUp = true;
                        ////WorldGen.mergeRight = true;
                    }
                    else if (up == current && down == tileToSearch && left == tileToSearch && right == current)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 36;
                            tile.frameY = 108;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 36;
                            tile.frameY = 144;
                        }
                        else
                        {
                            tile.frameX = 36;
                            tile.frameY = 180;
                        }

                        ////WorldGen.mergeDown = true;
                        ////WorldGen.mergeLeft = true;
                    }
                    else if (up == current && down == tileToSearch && left == current && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 54;
                            tile.frameY = 108;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 54;
                            tile.frameY = 144;
                        }
                        else
                        {
                            tile.frameX = 54;
                            tile.frameY = 180;
                        }

                        ////WorldGen.mergeDown = true;
                        ////WorldGen.mergeRight = true;
                    }
                    else if (up == current && down == current && left == tileToSearch && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 180;
                            tile.frameY = 126;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 180;
                            tile.frameY = 144;
                        }
                        else
                        {
                            tile.frameX = 180;
                            tile.frameY = 162;
                        }

                        //WorldGen.mergeLeft = true;
                        //WorldGen.mergeRight = true;
                    }
                    else if (up == tileToSearch && down == tileToSearch && left == current && right == current)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 144;
                            tile.frameY = 180;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 162;
                            tile.frameY = 180;
                        }
                        else
                        {
                            tile.frameX = 180;
                            tile.frameY = 180;
                        }

                        //WorldGen.mergeUp = true;
                        //WorldGen.mergeDown = true;
                    }
                    else if (up == tileToSearch && down == current && left == tileToSearch && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 198;
                            tile.frameY = 90;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 198;
                            tile.frameY = 108;
                        }
                        else
                        {
                            tile.frameX = 198;
                            tile.frameY = 126;
                        }

                        //WorldGen.mergeUp = true;
                        //WorldGen.mergeLeft = true;
                        //WorldGen.mergeRight = true;
                    }
                    else if (up == current && down == tileToSearch && left == tileToSearch && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 198;
                            tile.frameY = 144;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 198;
                            tile.frameY = 162;
                        }
                        else
                        {
                            tile.frameX = 198;
                            tile.frameY = 180;
                        }

                        //WorldGen.mergeDown = true;
                        //WorldGen.mergeLeft = true;
                        //WorldGen.mergeRight = true;
                    }
                    else if (up == tileToSearch && down == tileToSearch && left == current && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 216;
                            tile.frameY = 144;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 216;
                            tile.frameY = 162;
                        }
                        else
                        {
                            tile.frameX = 216;
                            tile.frameY = 180;
                        }

                        //WorldGen.mergeUp = true;
                        //WorldGen.mergeDown = true;
                        //WorldGen.mergeRight = true;
                    }
                    else if (up == tileToSearch && down == tileToSearch && left == tileToSearch && right == current)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 216;
                            tile.frameY = 90;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 216;
                            tile.frameY = 108;
                        }
                        else
                        {
                            tile.frameX = 216;
                            tile.frameY = 126;
                        }

                        //WorldGen.mergeUp = true;
                        //WorldGen.mergeDown = true;
                        //WorldGen.mergeLeft = true;
                    }
                    else if (up == tileToSearch && down == tileToSearch && left == tileToSearch && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 108;
                            tile.frameY = 198;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 126;
                            tile.frameY = 198;
                        }
                        else
                        {
                            tile.frameX = 144;
                            tile.frameY = 198;
                        }

                        //WorldGen.mergeUp = true;
                        //WorldGen.mergeDown = true;
                        //WorldGen.mergeLeft = true;
                        //WorldGen.mergeRight = true;
                    }
                    else if (up == current && down == current && left == current && right == current)
                    {
                        if (upLeft == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 18;
                                tile.frameY = 108;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 18;
                                tile.frameY = 144;
                            }
                            else
                            {
                                tile.frameX = 18;
                                tile.frameY = 180;
                            }
                        }

                        if (upRight == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 0;
                                tile.frameY = 108;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 0;
                                tile.frameY = 144;
                            }
                            else
                            {
                                tile.frameX = 0;
                                tile.frameY = 180;
                            }
                        }

                        if (downLeft == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 18;
                                tile.frameY = 90;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 18;
                                tile.frameY = 126;
                            }
                            else
                            {
                                tile.frameX = 18;
                                tile.frameY = 162;
                            }
                        }

                        if (downRight == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 0;
                                tile.frameY = 90;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 0;
                                tile.frameY = 126;
                            }
                            else
                            {
                                tile.frameX = 0;
                                tile.frameY = 162;
                            }
                        }
                    }
                }
                else
                {
                    if (!TileID.Sets.Grass[current] && !TileID.Sets.GrassSpecial[current])
                    {
                        if (up == -1 && down == tileToSearch && left == current && right == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 234;
                                tile.frameY = 0;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 252;
                                tile.frameY = 0;
                            }
                            else
                            {
                                tile.frameX = 270;
                                tile.frameY = 0;
                            }

                            //WorldGen.mergeDown = true;
                        }
                        else if (up == tileToSearch && down == -1 && left == current && right == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 234;
                                tile.frameY = 18;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 252;
                                tile.frameY = 18;
                            }
                            else
                            {
                                tile.frameX = 270;
                                tile.frameY = 18;
                            }

                            //WorldGen.mergeUp = true;
                        }
                        else if (up == current && down == current && left == -1 && right == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 234;
                                tile.frameY = 36;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 252;
                                tile.frameY = 36;
                            }
                            else
                            {
                                tile.frameX = 270;
                                tile.frameY = 36;
                            }

                            //WorldGen.mergeRight = true;
                        }
                        else if (up == current && down == current && left == tileToSearch && right == -1)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 234;
                                tile.frameY = 54;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 252;
                                tile.frameY = 54;
                            }
                            else
                            {
                                tile.frameX = 270;
                                tile.frameY = 54;
                            }

                            //WorldGen.mergeLeft = true;
                        }
                    }

                    if (up != -1 && down != -1 && left == -1 && right == current)
                    {
                        if (up == tileToSearch && down == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 72;
                                tile.frameY = 144;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 72;
                                tile.frameY = 162;
                            }
                            else
                            {
                                tile.frameX = 72;
                                tile.frameY = 180;
                            }

                            //WorldGen.mergeUp = true;
                        }
                        else if (down == tileToSearch && up == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 72;
                                tile.frameY = 90;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 72;
                                tile.frameY = 108;
                            }
                            else
                            {
                                tile.frameX = 72;
                                tile.frameY = 126;
                            }

                            //WorldGen.mergeDown = true;
                        }
                    }
                    else if (up != -1 && down != -1 && left == current && right == -1)
                    {
                        if (up == tileToSearch && down == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 90;
                                tile.frameY = 144;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 90;
                                tile.frameY = 162;
                            }
                            else
                            {
                                tile.frameX = 90;
                                tile.frameY = 180;
                            }

                            //WorldGen.mergeUp = true;
                        }
                        else if (down == tileToSearch && up == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 90;
                                tile.frameY = 90;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 90;
                                tile.frameY = 108;
                            }
                            else
                            {
                                tile.frameX = 90;
                                tile.frameY = 126;
                            }

                            //WorldGen.mergeDown = true;
                        }
                    }
                    else if (up == -1 && down == current && left != -1 && right != -1)
                    {
                        if (left == tileToSearch && right == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 0;
                                tile.frameY = 198;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 18;
                                tile.frameY = 198;
                            }
                            else
                            {
                                tile.frameX = 36;
                                tile.frameY = 198;
                            }

                            //WorldGen.mergeLeft = true;
                        }
                        else if (right == tileToSearch && left == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 54;
                                tile.frameY = 198;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 72;
                                tile.frameY = 198;
                            }
                            else
                            {
                                tile.frameX = 90;
                                tile.frameY = 198;
                            }

                            //WorldGen.mergeRight = true;
                        }
                    }
                    else if (up == current && down == -1 && left != -1 && right != -1)
                    {
                        if (left == tileToSearch && right == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 0;
                                tile.frameY = 216;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 18;
                                tile.frameY = 216;
                            }
                            else
                            {
                                tile.frameX = 36;
                                tile.frameY = 216;
                            }

                            //WorldGen.mergeLeft = true;
                        }
                        else if (right == tileToSearch && left == current)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 54;
                                tile.frameY = 216;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 72;
                                tile.frameY = 216;
                            }
                            else
                            {
                                tile.frameX = 90;
                                tile.frameY = 216;
                            }

                            //WorldGen.mergeRight = true;
                        }
                    }
                    else if (up != -1 && down != -1 && left == -1 && right == -1)
                    {
                        if (up == tileToSearch && down == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 108;
                                tile.frameY = 216;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 108;
                                tile.frameY = 234;
                            }
                            else
                            {
                                tile.frameX = 108;
                                tile.frameY = 252;
                            }

                            //WorldGen.mergeUp = true;
                            //WorldGen.mergeDown = true;
                        }
                        else if (up == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 126;
                                tile.frameY = 144;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 126;
                                tile.frameY = 162;
                            }
                            else
                            {
                                tile.frameX = 126;
                                tile.frameY = 180;
                            }

                            //WorldGen.mergeUp = true;
                        }
                        else if (down == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 126;
                                tile.frameY = 90;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 126;
                                tile.frameY = 108;
                            }
                            else
                            {
                                tile.frameX = 126;
                                tile.frameY = 126;
                            }

                            //WorldGen.mergeDown = true;
                        }
                    }
                    else if (up == -1 && down == -1 && left != -1 && right != -1)
                    {
                        if (left == tileToSearch && right == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 162;
                                tile.frameY = 198;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 180;
                                tile.frameY = 198;
                            }
                            else
                            {
                                tile.frameX = 198;
                                tile.frameY = 198;
                            }

                            //WorldGen.mergeLeft = true;
                            //WorldGen.mergeRight = true;
                        }
                        else if (left == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 0;
                                tile.frameY = 252;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 18;
                                tile.frameY = 252;
                            }
                            else
                            {
                                tile.frameX = 36;
                                tile.frameY = 252;
                            }

                            //WorldGen.mergeLeft = true;
                        }
                        else if (right == tileToSearch)
                        {
                            if (current63 == 0)
                            {
                                tile.frameX = 54;
                                tile.frameY = 252;
                            }
                            else if (current63 == 1)
                            {
                                tile.frameX = 72;
                                tile.frameY = 252;
                            }
                            else
                            {
                                tile.frameX = 90;
                                tile.frameY = 252;
                            }

                            //WorldGen.mergeRight = true;
                        }
                    }
                    else if (up == tileToSearch && down == -1 && left == -1 && right == -1)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 108;
                            tile.frameY = 144;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 108;
                            tile.frameY = 162;
                        }
                        else
                        {
                            tile.frameX = 108;
                            tile.frameY = 180;
                        }

                        //WorldGen.mergeUp = true;
                    }
                    else if (up == -1 && down == tileToSearch && left == -1 && right == -1)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 108;
                            tile.frameY = 90;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 108;
                            tile.frameY = 108;
                        }
                        else
                        {
                            tile.frameX = 108;
                            tile.frameY = 126;
                        }

                        //WorldGen.mergeDown = true;
                    }
                    else if (up == -1 && down == -1 && left == tileToSearch && right == -1)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 0;
                            tile.frameY = 234;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 18;
                            tile.frameY = 234;
                        }
                        else
                        {
                            tile.frameX = 36;
                            tile.frameY = 234;
                        }

                        //WorldGen.mergeLeft = true;
                    }
                    else if (up == -1 && down == -1 && left == -1 && right == tileToSearch)
                    {
                        if (current63 == 0)
                        {
                            tile.frameX = 54;
                            tile.frameY = 234;
                        }
                        else if (current63 == 1)
                        {
                            tile.frameX = 72;
                            tile.frameY = 234;
                        }
                        else
                        {
                            tile.frameX = 90;
                            tile.frameY = 234;
                        }

                        //WorldGen.mergeRight = true;
                    }
                }
            }
        }

        public static void RegularMerge(int i, int j)
        {
            int current = Main.tile[i, j].type;
            int up = Main.tile[i, j - 1].type;
            int down = Main.tile[i, j + 1].type;
            int left = Main.tile[i - 1, j].type;
            int right = Main.tile[i + 1, j].type;
            int upLeft = Main.tile[i - 1, j + 1].type;
            int upRight = Main.tile[i + 1, j + 1].type;
            int downLeft = Main.tile[i - 1, j - 1].type;
            int downRight = Main.tile[i + 1, j - 1].type;

            int current63 = WorldGen.genRand.Next(0, 3);

            Tile tile = Main.tile[i, j];

            //WorldGen.TileMergeAttempt(Type, mod.TileType("Wasteland_Dirt"), ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);

            if (up == current && down == current && left == current && right == current)
            {
                if (upLeft != current && upRight != current)
                {
                    if (current63 == 0)
                    {
                        tile.frameX = 108;
                        tile.frameY = 18;
                    }
                    else if (current63 == 1)
                    {
                        tile.frameX = 126;
                        tile.frameY = 18;
                    }
                    else
                    {
                        tile.frameX = 144;
                        tile.frameY = 18;
                    }
                }
                else if (downLeft != current && downRight != current)
                {
                    if (current63 == 0)
                    {
                        tile.frameX = 108;
                        tile.frameY = 36;
                    }
                    else if (current63 == 1)
                    {
                        tile.frameX = 126;
                        tile.frameY = 36;
                    }
                    else
                    {
                        tile.frameX = 144;
                        tile.frameY = 36;
                    }
                }
                else if (upLeft != current && downLeft != current)
                {
                    if (current63 == 0)
                    {
                        tile.frameX = 180;
                        tile.frameY = 0;
                    }
                    else if (current63 == 1)
                    {
                        tile.frameX = 180;
                        tile.frameY = 18;
                    }
                    else
                    {
                        tile.frameX = 180;
                        tile.frameY = 36;
                    }
                }
                else if (upRight != current && downRight != current)
                {
                    if (current63 == 0)
                    {
                        tile.frameX = 198;
                        tile.frameY = 0;
                    }
                    else if (current63 == 1)
                    {
                        tile.frameX = 198;
                        tile.frameY = 18;
                    }
                    else
                    {
                        tile.frameX = 198;
                        tile.frameY = 36;
                    }
                }
                else if (current63 == 0)
                {
                    tile.frameX = 18;
                    tile.frameY = 18;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 36;
                    tile.frameY = 18;
                }
                else
                {
                    tile.frameX = 54;
                    tile.frameY = 18;
                }
            }
            else if (up != current && down == current && left == current && right == current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 18;
                    tile.frameY = 0;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 36;
                    tile.frameY = 0;
                }
                else
                {
                    tile.frameX = 54;
                    tile.frameY = 0;
                }
            }
            else if (up == current && down != current && left == current && right == current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 18;
                    tile.frameY = 36;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 36;
                    tile.frameY = 36;
                }
                else
                {
                    tile.frameX = 54;
                    tile.frameY = 36;
                }
            }
            else if (up == current && down == current && left != current && right == current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 0;
                    tile.frameY = 0;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 0;
                    tile.frameY = 18;
                }
                else
                {
                    tile.frameX = 0;
                    tile.frameY = 36;
                }
            }
            else if (up == current && down == current && left == current && right != current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 72;
                    tile.frameY = 0;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 72;
                    tile.frameY = 18;
                }
                else
                {
                    tile.frameX = 72;
                    tile.frameY = 36;
                }
            }
            else if (up != current && down == current && left != current && right == current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 0;
                    tile.frameY = 54;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 36;
                    tile.frameY = 54;
                }
                else
                {
                    tile.frameX = 72;
                    tile.frameY = 54;
                }
            }
            else if (up != current && down == current && left == current && right != current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 18;
                    tile.frameY = 54;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 54;
                    tile.frameY = 54;
                }
                else
                {
                    tile.frameX = 90;
                    tile.frameY = 54;
                }
            }
            else if (up == current && down != current && left != current && right == current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 0;
                    tile.frameY = 72;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 36;
                    tile.frameY = 72;
                }
                else
                {
                    tile.frameX = 72;
                    tile.frameY = 72;
                }
            }
            else if (up == current && down != current && left == current && right != current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 18;
                    tile.frameY = 72;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 54;
                    tile.frameY = 72;
                }
                else
                {
                    tile.frameX = 90;
                    tile.frameY = 72;
                }
            }
            else if (up == current && down == current && left != current && right != current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 90;
                    tile.frameY = 0;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 90;
                    tile.frameY = 18;
                }
                else
                {
                    tile.frameX = 90;
                    tile.frameY = 36;
                }
            }
            else if (up != current && down != current && left == current && right == current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 108;
                    tile.frameY = 72;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 126;
                    tile.frameY = 72;
                }
                else
                {
                    tile.frameX = 144;
                    tile.frameY = 72;
                }
            }
            else if (up != current && down == current && left != current && right != current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 108;
                    tile.frameY = 0;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 126;
                    tile.frameY = 0;
                }
                else
                {
                    tile.frameX = 144;
                    tile.frameY = 0;
                }
            }
            else if (up == current && down != current && left != current && right != current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 108;
                    tile.frameY = 54;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 126;
                    tile.frameY = 54;
                }
                else
                {
                    tile.frameX = 144;
                    tile.frameY = 54;
                }
            }
            else if (up != current && down != current && left != current && right == current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 162;
                    tile.frameY = 0;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 162;
                    tile.frameY = 18;
                }
                else
                {
                    tile.frameX = 162;
                    tile.frameY = 36;
                }
            }
            else if (up != current && down != current && left == current && right != current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 216;
                    tile.frameY = 0;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 216;
                    tile.frameY = 18;
                }
                else
                {
                    tile.frameX = 216;
                    tile.frameY = 36;
                }
            }
            else if (up != current && down != current && left != current && right != current)
            {
                if (current63 == 0)
                {
                    tile.frameX = 162;
                    tile.frameY = 54;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 180;
                    tile.frameY = 54;
                }
                else
                {
                    tile.frameX = 198;
                    tile.frameY = 54;
                }
            }

            if (tile.frameX <= -1 || tile.frameY <= -1)
            {
                if (current63 <= 0)
                {
                    tile.frameX = 18;
                    tile.frameY = 18;
                }
                else if (current63 == 1)
                {
                    tile.frameX = 36;
                    tile.frameY = 18;
                }

                if (current63 >= 2)
                {
                    tile.frameX = 54;
                    tile.frameY = 18;
                }
            }
        }

        public static bool CanPlaceTile(int x, int y, int width, int height)
        {
            for (int i = x; i < x + width - 1; i++)
            {
                for (int j = y; j < y + height - 1; j++)
                {
                    if (Main.tile[i, j].active())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void Fill(int x, int startingY, int width, int depth, ushort tile)
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

        public static void FillInvertYAxis(int x, int startingY, int width, int depth, ushort tile)
        {
            for (int i = startingY; i > startingY - depth; i--)
            {
                if (!WorldGen.InWorld(x, i))
                {
                    return;
                }
                WorldGen.PlaceTile(x, i, tile, false, true);
            }
        }

        public static void FillAir(int x, int depth, int width, int startingY)
        {
            for (int i = startingY; i < startingY + depth; i++)
            {
                Main.tile[x, i].active(false);
            }
        }
    }
}
