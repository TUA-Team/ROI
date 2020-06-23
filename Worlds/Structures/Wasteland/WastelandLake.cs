using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiquidAPI;
using LiquidAPI.LiquidMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.Worlds.Structures.Wasteland
{
    class WastelandLake
    {
        internal static void Generate(Vector2 fromEntityPosition)
        {
            Generate((int) (fromEntityPosition.X / 16), (int) (fromEntityPosition.Y / 16));
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
            int xModifier = WorldGen.genRand.Next(20, 25);
            int maxY = ParabolaEquation(curveModifier, WorldGen.genRand.Next(10, 15), xModifier);
            for (int i = -xModifier; i < xModifier; i++)
            {
                int minY = ParabolaEquation(curveModifier, 5, i);
                for (int j = maxY; j > minY; j--)
                {
                    if (j > minY + 2)
                    {
                        ushort tilePlace = (ushort) ((WorldGen.genRand.Next(4) == 0) ? ROIMod.instance.TileType("Wasteland_Waste") : ROIMod.instance.TileType("Wasteland_Rock"));
                        ushort tilePlace2 = (ushort) ((WorldGen.genRand.Next(4) == 0) ? ROIMod.instance.TileType("Wasteland_Waste") : ROIMod.instance.TileType("Wasteland_Rock"));
                        ushort tilePlace3 = (ushort) ((WorldGen.genRand.Next(4) == 0) ? ROIMod.instance.TileType("Wasteland_Waste") : ROIMod.instance.TileType("Wasteland_Rock"));
                        WorldGen.PlaceTile(x + i, y + j, tilePlace);
                    }
                    else
                    {
                        LiquidRef liquid = LiquidWorld.grid[x + i, y + j];
                        liquid.Amount = 255;
                        liquid.Type = LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "PlutonicWaste");
                        Main.tile[x + i, y + j].wall = (ushort) ROIMod.instance.WallType("Wasteland_Rock_Wall");
                        WorldGen.SquareTileFrame(x + i, y + j, true);
                    }
                    
                }
            }
        }


        //TO DO : Move in Math Helper
        internal static int ParabolaEquation(float curveModifier, float heightModifier, int x)
        {
            return (int) (curveModifier * (x * x) + heightModifier);
        }

    }
}
