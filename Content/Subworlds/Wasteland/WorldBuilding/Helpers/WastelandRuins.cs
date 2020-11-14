using API;
using Microsoft.Xna.Framework;
using ROI.Content.Subworlds.Wasteland.Furniture.Tiles;
using ROI.Content.Subworlds.Wasteland.Furniture.Walls;
using ROI.Content.Subworlds.Wasteland.WorldBuilding.Tiles;
using ROI.Worlds;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding.Helpers
{
    public static class WastelandRuins
    {
        public static void PlaceHouse(int x, int y)
        {
            int floor = WorldGen.genRand.Next(5, 10);
            int height = WorldGen.genRand.Next(7, 10);
            int width = WorldGen.genRand.Next(10, 15);

            List<Rectangle> roomLocation = new List<Rectangle>();

            for (int currentFloor = 0; currentFloor < floor; currentFloor++)
            {
                //height = WorldGen.genRand.Next(6, 10);
                GenerateHouse(currentFloor, floor, width, height, x, y);
                roomLocation.Add(new Rectangle(x, y, width, height));

                if (WorldGen.genRand.Next(5) == 0)
                {
                    width = WorldGen.genRand.Next(20, 25);
                    x -= width - 1;
                }
                else
                {
                    height = WorldGen.genRand.Next(9, 12);
                    y -= height - 1;
                }
            }

            for (int i = 0; i < roomLocation.Count; i++)
            {
                if (i < roomLocation.Count - 2)
                    GenerateDoor(roomLocation[i], roomLocation[i + 1]);

            }

            for (int i = 0; i < roomLocation.Count; i++)
            {
                GenerateChest(roomLocation[i]);
            }

            foreach (Rectangle rectangle in roomLocation)
            {
                List<Rectangle> roomListWithoutCurrent = new List<Rectangle>(roomLocation);
                roomListWithoutCurrent.Remove(rectangle);

                Point bottomLeft = rectangle.BottomLeft().ToPoint();
                bottomLeft.X += 2;
                if (!Main.tile[bottomLeft.X, bottomLeft.Y].active())
                {
                    int columnStart = bottomLeft.Y;
                    while (!Main.tile[bottomLeft.X, columnStart].active() && WorldGen.InWorld(bottomLeft.X, columnStart))
                    {
                        WorldGen.PlaceTile(bottomLeft.X, columnStart, TileID.WoodenBeam);
                    }
                }

            }
        }

        public static void GenerateDoor(Rectangle roomLocation, Rectangle nextRoom)
        {
            if (nextRoom.IntersectsWith(roomLocation))
            {
                for (int i = roomLocation.Y; i < roomLocation.Y + roomLocation.Height; i++)
                {
                    if (nextRoom.Contains(roomLocation.X, roomLocation.Y + roomLocation.Height - 3))
                    {
                        WorldGen.KillTile(roomLocation.X, roomLocation.Y + roomLocation.Height - 2);
                        WorldGen.KillTile(roomLocation.X, roomLocation.Y + roomLocation.Height - 3);
                        WorldGen.KillTile(roomLocation.X, roomLocation.Y + roomLocation.Height - 4);
                        WorldGen.PlaceObject(roomLocation.X, roomLocation.Y + roomLocation.Height - 3, ModContent.TileType<WastebrickDoorClosed>());
                    }
                }
            }
        }

        public static void GenerateChest(Rectangle roomLocation)
        {
            for (int i = roomLocation.X; i < roomLocation.X + roomLocation.Width; i++)
            {
                for (int j = roomLocation.Y; j < roomLocation.Y + roomLocation.Height; j++)
                {
                    //Chest
                    if (GeneralWorldHelper.CanPlaceTile(i, j, 2, 2) && WorldGen.genRand.Next(20) == 0)
                    {
                        int chestID = WorldGen.PlaceChest(i, j, (ushort)ModContent.TileType<IrradiatedChest>(), false, 1);
                        if (chestID != -1)
                        {
                            //TODO: Proper loot
                            Chest chest = Main.chest[chestID];
                            chest.item[0].SetDefaults(ItemID.Actuator);
                        }
                    }
                }
            }
        }

        private static void GenerateHouse(int currentFloor, int maxFloor, int width, int height, int x, int y)
        {
            for (int i = y; i < y + height; i++)
            {
                if (WorldGen.InWorld(x, i) && WorldGen.InWorld(x + width - 1, i))
                {
                    Main.tile[x, i].active(true);
                    Main.tile[x + width - 1, i].active(true);
                    Main.tile[x, i].type = (ushort)ModContent.TileType<WastelandBrick>();
                    Main.tile[x + width - 1, i].type = (ushort)ModContent.TileType<WastelandBrick>();
                    //WorldGen.SquareTileFrame(x, i);
                    //WorldGen.SquareTileFrame(x + width - 1, i);
                }
            }

            for (int i = x + 1; i < x + width - 1; i++)
            {
                for (int j = y + 1; j < y + height - 1; j++)
                {
                    Main.tile[i, j].active(false);
                    if (WorldGen.genRand.Next(10) == 0)
                        continue;

                    Main.tile[i, j].wall = (ushort)ModContent.WallType<WastestoneBrickWall>();
                    //WorldGen.SquareWallFrame(i, j);
                }
            }

            for (int i = x + 1; i < x + width - 1; i++)
            {
                int v = y + height - 1;
                if (!WorldGen.InWorld(i, v))
                    continue;
                if (currentFloor == 0)
                {
                    WorldGen.PlaceTile(i, v, ModContent.TileType<WastelandBrick>(), true, true);
                    continue;
                }
                if (WorldGen.genRand.Next(8) == 0)
                    continue;
                //Main.tile[i, v].active(true);

                if (Main.tile[i, v + 1].wall == ModContent.WallType<WastestoneBrickWall>() && Main.tile[i, v].type != (ushort)ModContent.TileType<WastelandBrick>())
                {
                    // TODO: WorldGen.PlaceTile(i, v, ModContent.TileType<Wastebrick_Platform>(), true, true);
                    //WorldGen.PlaceTile(i, v, ModContent.TileType<Wastebrick_Platform>(), true, true);
                    Main.tile[i, v].active(true);
                    Main.tile[i, v].type = (ushort)ModContent.TileType<Wastebrick_Platform>();
                }
                //WorldGen.SquareTileFrame(i, y + height);
            }

            for (int i = x + 1; i < x + width - 1; i++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                {
                    Main.tile[i, y].active(true);
                    Main.tile[i, y].type = (ushort)ModContent.TileType<WastelandBrick>();
                    //WorldGen.SquareTileFrame(i, y);
                }
            }

            if (currentFloor == maxFloor - 1)
            {
                for (int i = x + 1; i < x + width - 1; i++)
                {
                    if (WorldGen.genRand.Next(20) == 0)
                    {
                        //WorldGen.SquareTileFrame(i, y);
                        WorldGen.PlaceTile(i, y, ModContent.TileType<Wastebrick_Platform>(), true, true);
                    }
                }
            }
        }
    }
}
