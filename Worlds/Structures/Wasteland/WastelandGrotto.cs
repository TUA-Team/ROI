using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace ROI.Worlds.Structures.Wasteland
{
    class WastelandGrotto
    {
        internal static int numberOfGrotto = 0;

        internal static void Generate(int x, int y)
        {

            x /= 16;
            y /= 16;
            x = Math.Abs(x);
            y = Math.Abs(y);
            float curveModifier = WorldGen.genRand.NextFloat(0.01f, 0.03f);
            int xModifier = WorldGen.genRand.Next(65, 70);
            int maxY = ParabolaEquation(curveModifier, WorldGen.genRand.Next(10, 15), xModifier);
            for (int i = -xModifier; i < xModifier; i++)
            {
                int minY = ParabolaEquation(curveModifier, 5, i);
                for (int j = minY; j < maxY; j++)
                {
                    WorldGen.PlaceTile(x + i, y + j, ROIMod.instance.TileType("Wasteland_Rock"));
                    Main.tile[x + i, y + j].active(true);
                    Main.tile[x + i, y + j].type = (ushort) ROIMod.instance.TileType("Wasteland_Rock");
                }
            }
        }

        internal static int ParabolaEquation(float curveModifier, float heightModifier, int x)
        {
            return (int) (curveModifier * (x * x) + heightModifier);
        }
    }
}
