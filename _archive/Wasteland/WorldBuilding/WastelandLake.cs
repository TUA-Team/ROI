﻿using Microsoft.Xna.Framework;
using ROI.Content.Subworlds.Wasteland.WorldBuilding.Tiles;
using ROI.Worlds;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding.Helpers
{
    internal static class WastelandLake
    {
        internal static void Generate(Vector2 fromEntityPosition)
        {
            Generate((int)(fromEntityPosition.X / 16), (int)(fromEntityPosition.Y / 16), 40, 55);
        }

        internal static Rectangle Generate(int x, int y, int minRadius, int maxRadius, float strengh = 5f, int steps = 3, int overflow = 0, bool dirt = false)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            float curveModifier = WorldGen.genRand.NextFloat(-0.01f, -0.03f);
            int xModifier = WorldGen.genRand.Next(minRadius, maxRadius);
            //int maxY = ParabolaEquation(curveModifier, WorldGen.genRand.Next(30, 55), xModifier);
            int maxY = y + xModifier / 2 * xModifier / 2 / 25 * -1;
            y += y + 0 * 0 / 25 * -1 - maxY;

            for (int i = -xModifier / 2; i < xModifier / 2; i++)
            {
                Point point = new Point(x + i, y + i * i / 25 * -1);
                GeneralWorldHelper.FillAir(point.X, maxY, 1, point.Y - maxY);
            }

            for (int i = -xModifier / 2; i < xModifier / 2; i++)
            {
                Point point = new Point(x + i, y + i * i / 25 * -1);
                ushort tilePlace = (ushort)(WorldGen.genRand.Next(4) == 0 ? ModContent.TileType<WastelandWaste>() : ModContent.TileType<WastelandRock>());
                if (!dirt)
                    WorldGen.TileRunner(point.X, point.Y, strengh, steps, ModContent.TileType<WastelandRock>(), true);
            }

            for (int i = -(xModifier + 7) / 2; i < (xModifier + 7) / 2; i++)
            {
                Point point = new Point(x + i, y + i * i / 25 * -1);
                if (dirt)
                    GeneralWorldHelper.FillTile(point.X, maxY, 1, point.Y - maxY, new ushort[] { (ushort)ModContent.TileType<WastelandRock>(), (ushort)ModContent.TileType<WastelandDirt>() }, new ushort[] { 2, 4 }, true);
                else
                    GeneralWorldHelper.FillTile(point.X, maxY, 1, point.Y - maxY, new ushort[] { (ushort)ModContent.TileType<WastelandRock>(), (ushort)ModContent.TileType<WastelandWaste>() }, new ushort[] { 4, 2 }, true);
                //GeneralWorldHelper.FillLiquid(point.X, maxY, 1, point.Y - maxY, LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "PlutonicWaste"), true);
            }

            for (int i = -xModifier / 2; i < xModifier / 2; i++)
            {
                Point point = new Point(x + i, y + i * i / 25 * -1);
            }
            return new Rectangle(x - xModifier / 2, y, xModifier, y - maxY);
        }



        // TODO: Move in Math Helper
        internal static int ParabolaEquation(float curveModifier, float heightModifier, int x)
        {
            return (int)(curveModifier * (x * x) + heightModifier);
        }
    }
}
