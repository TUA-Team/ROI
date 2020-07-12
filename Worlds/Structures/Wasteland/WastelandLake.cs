using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiquidAPI;
using LiquidAPI.LiquidMod;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using ROI.Helpers;
using ROI.Tiles.Wasteland;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.Worlds.Structures.Wasteland
{
    class WastelandLake
    {
        internal static void Generate(Vector2 fromEntityPosition)
        {
            Generate((int)(fromEntityPosition.X / 16), (int)(fromEntityPosition.Y / 16));
        }
#if DEBUG
        internal static void Generate(Point16 position)
        {
            Generate(position.X, position.Y);
        }
#endif

        internal static void Generate(int x, int y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            float curveModifier = WorldGen.genRand.NextFloat(-0.01f, -0.03f);
            int xModifier = WorldGen.genRand.Next(40, 55);
            //int maxY = ParabolaEquation(curveModifier, WorldGen.genRand.Next(30, 55), xModifier);
            int maxY = y + xModifier / 2 * xModifier / 2 / 25 * -1;
            //y += (y + 0 * 0 / 25 * -1) - maxY;
            for (int i = -(xModifier) / 2; i < (xModifier ) / 2; i++)
            {
                Point point = new Point(x + i, y + i * i / 25 * -1);
                ushort tilePlace = (ushort)((WorldGen.genRand.Next(4) == 0) ? ROIMod.instance.TileType("Wasteland_Waste") : ROIMod.instance.TileType("Wasteland_Rock"));
                WorldGen.TileRunner(point.X, point.Y, 8f, 3, ROIMod.instance.TileType("Wasteland_Rock"), true);
            }

            for (int i = -(xModifier) / 2; i < (xModifier) / 2; i++)
            {
                Point point = new Point(x + i, y + i * i / 25 * -1);
                ROIWorldGenHelper.FillTile(point.X, maxY, 1, point.Y - maxY, new ushort[] {(ushort) ModContent.TileType<Wasteland_Rock>(), (ushort) ModContent.TileType<Wasteland_Waste>()}, new ushort[] {4, 2}, true);
            }

            for (int i = -(xModifier - 5) / 2; i < (xModifier - 5) / 2; i++)
            {
                Point point = new Point(x + i, y + i * i / 25 * -1);
                ROIWorldGenHelper.FillLiquid(point.X, maxY, 1, point.Y - maxY, LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "PlutonicWaste"), true);
            }
        }

        

        //TO DO : Move in Math Helper
        internal static int ParabolaEquation(float curveModifier, float heightModifier, int x)
        {
            return (int)(curveModifier * (x * x) + heightModifier);
        }

    }
}
