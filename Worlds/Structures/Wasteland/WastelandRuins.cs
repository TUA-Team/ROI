using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.Tiles.Furniture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Worlds.Structures
{
    class WastelandRuins
    {
        internal static ROIMod mod => ROIMod.instance;

        public static void PlaceHouse(int x, int y)
        {

            int floor = WorldGen.genRand.Next(5, 10);
            int height = WorldGen.genRand.Next(8, 11);
            int width = WorldGen.genRand.Next(20, 25);

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
        }

        public static void GenerateDoor(Rectangle roomLocation, Rectangle nextRoom)
        {
            if (nextRoom.Intersects(roomLocation))
            {
                for (int i = roomLocation.Y; i < roomLocation.Y + roomLocation.Height; i++)
                {
                    if (nextRoom.Contains(roomLocation.X, roomLocation.Y + roomLocation.Height - 3))
                    {
                        WorldGen.KillTile(roomLocation.X, roomLocation.Y + roomLocation.Height - 2);
                        WorldGen.KillTile(roomLocation.X, roomLocation.Y + roomLocation.Height - 3);
                        WorldGen.KillTile(roomLocation.X, roomLocation.Y + roomLocation.Height - 4);
                        WorldGen.PlaceObject(roomLocation.X, roomLocation.Y + roomLocation.Height - 3, mod.TileType("Wastebrick_Door_Closed"));

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
                    if (ROIWorldHelper.CanPlaceTile(i, j, 2, 2) && WorldGen.genRand.Next(20) == 0)
                    {
                        int chestID = WorldGen.PlaceChest(i, j, (ushort)mod.TileType("Irradiated_Chest"), false, 1);
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
                    Main.tile[x, i].type = (ushort)mod.TileType("Wasteland_Brick");
                    Main.tile[x + width - 1, i].type = (ushort)mod.TileType("Wasteland_Brick");
                    //WorldGen.SquareTileFrame(x, i);
                    //WorldGen.SquareTileFrame(x + width - 1, i);

                }
            }

            for (int i = x + 1; i < x + width - 1; i++)
            {
                for (int j = y + 1; j < y + height - 1; j++)
                {
                    if (WorldGen.genRand.Next(10) == 0)
                        continue;
                    Main.tile[i, j].active(false);
                    Main.tile[i, j].wall = (ushort)mod.WallType("WastestoneBrickWall");
                    //WorldGen.SquareWallFrame(i, j);
                }
            }


            bool hitBrick = false;
            for (int i = x + 1; i < x + width - 1; i++)
            {
                if (currentFloor == 0)
                {
                    Main.tile[i, y + height - 1].active(true);
                    Main.tile[i, y + height - 1].type = (ushort)mod.TileType("Wasteland_Brick");
                    continue;
                }
                if (WorldGen.genRand.Next(8) == 0)
                    continue;
                Main.tile[i, y + height - 1].active(true);

                if (Main.tile[i, y + height - 1].type == (ushort)mod.TileType("Wasteland_Brick"))
                    hitBrick = true;
                if (!hitBrick)
                    Main.tile[i, y + height - 1].type = (ushort)mod.TileType("Wastebrick_Platform");
                else
                    Main.tile[i, y + height - 1].type = (ushort)mod.TileType("Wasteland_Brick");
                //WorldGen.SquareTileFrame(i, y + height);
            }

            for (int i = x + 1; i < x + width - 1; i++)
            {

                if (WorldGen.genRand.Next(20) == 0)
                {
                    Main.tile[i, y].active(true);
                    Main.tile[i, y].type = (ushort)mod.TileType("Wasteland_Brick");
                    //WorldGen.SquareTileFrame(i, y);
                }

            }

            if (currentFloor == maxFloor - 1)
            {
                for (int i = x + 1; i < x + width - 1; i++)
                {

                    if (WorldGen.genRand.Next(20) == 0)
                    {
                        Main.tile[i, y].active(true);
                        Main.tile[i, y].type = (ushort)ModContent.TileType<Wastebrick_Platform>();
                        //WorldGen.SquareTileFrame(i, y);
                    }

                }
            }
        }
    }
}
