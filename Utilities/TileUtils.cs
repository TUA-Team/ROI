using System;
using Terraria;

namespace ROI.Utilities
{
    public static class TileUtils
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

        public static void TileMergeAttempt(ushort self, ushort merge, int i, int j)
        {
            int up = Main.tile[i, j - 1].type;
            int down = Main.tile[i, j + 1].type;
            int left = Main.tile[i - 1, j].type;
            int right = Main.tile[i + 1, j].type;

            WorldGen.TileMergeAttempt(self, merge, ref up, ref down, ref left, ref right);
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
    }
}
