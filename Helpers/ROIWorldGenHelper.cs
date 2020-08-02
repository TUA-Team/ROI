using LiquidAPI;
using LiquidAPI.LiquidMod;
using System.Collections.Generic;
using Terraria;

namespace ROI.Helpers
{
    static class ROIWorldGenHelper
    {
        public static void FillTile(int i, int j, int width, int height, ushort[] tile, ushort[] weight, bool replaceTileMode = false)
        {
            if (tile.Length != weight.Length) return;

            List<ushort> weightedList = new List<ushort>();

            for (int index = 0; index < weight.Length; index++)
            {
                for (int amountOfItem = 0; amountOfItem < weight[index]; amountOfItem++)
                {
                    weightedList.Add(tile[index]);
                }
            }

            for (int x = i; x < i + width; x++)
            {
                for (int y = j; y < j + height; y++)
                {
                    if (replaceTileMode && Main.tile[x, y].active())
                    {
                        Main.tile[x, y].type = WorldGen.genRand.Next(weightedList);
                        WorldGen.SquareTileFrame(x, y);
                    }
                    else if (!replaceTileMode)
                    {
                        Main.tile[x, y].active(true);
                        Main.tile[x, y].type = WorldGen.genRand.Next(weightedList);
                        WorldGen.SquareTileFrame(x, y);
                    }
                }
            }
        }

        public static void FillLiquid(int i, int j, int width, int height, ModLiquid liquidID, bool stopIfTileHit = false)
        {
            for (int x = i; x < i + width; x++)
            {
                for (int y = j; y < j + height; y++)
                {
                    if (Main.tile[i, j].active() && stopIfTileHit)
                    {
                        continue;
                    }
                    else
                    {
                        LiquidRef liquidRef = LiquidWorld.grid[x, y];
                        liquidRef.Amount = 255;
                        liquidRef.Type = liquidID;
                    }
                }
            }
        }

        public static void FillAir(int i, int j, int width, int height)
        {
            for (int x = i; x < i + width; x++)
            {
                for (int y = j; y < j + height; y++)
                {
                    Main.tile[x, y].active(false);
                }
            }
        }
    }
}
