using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Worlds.Underworld
{
    internal class ROIWorldHelper
    {

        public static void TileRunner(int i, int j, double strength, int steps, int type, bool addTile = false, float speedX = 0f, float speedY = 0f, bool noYChange = false, bool overRide = true)
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
                                        //TODO: Introduce Unstable liquid goo
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
        }
    }
}
