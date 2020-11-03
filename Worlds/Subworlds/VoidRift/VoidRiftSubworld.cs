using System.Collections.Generic;
using Bubble_Defender.Algorithm;
using Microsoft.Xna.Framework;
using ROI.Tiles.VoidRift;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Worlds.Subworlds
{
    public class VoidRiftSubworld : Subworld
    {
        public override int width => 7200;
        public override int height => 1800;
        public override List<GenPass> tasks => GeneratePassList();

        public override ModWorld modWorld => ModContent.GetInstance<VoidRiftModWorld>();

        public override bool saveSubworld => false;
        public override bool disablePlayerSaving => true;

        public Information.FloorInfo pendingFloorInfo;

        public Point[] possibleNormalRoomSize = {
            new Point(75, 75),
            new Point(100, 75),
            new Point(100, 100),
            new Point(150, 75),
            new Point(150, 150)
        };

        public Point[] possibleBossSizeRoom =
        {
            new Point(150, 150), //The void triplet room?
            new Point(300, 150) //Tentacle boss?
        };

        public Point startingRoomSize = new Point(100, 100);

        public List<GenPass> GeneratePassList()
        {

            return new List<GenPass>()
            {
                new PassLegacy("Fill the world", progress =>
                {
                    progress.Message = "Loading"; //Sets the text above the worldgen progress bar
                    Main.worldSurface = Main.maxTilesY - 42; //Hides the underground layer just out of bounds
                    Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        for (int j = 0; j < Main.maxTilesY; j++)
                        {
                            progress.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY)); //Controls the progress bar, should only be set between 0f and 1f
                            Main.tile[i, j].active(true);
                            Main.tile[i, j].type = ROIUtils.TileType<Void_Brick>();
                        }
                    }

                    if (VoidRiftModWorld.currentDungeon == null)
                    {
                        VoidRiftModWorld.currentDungeon = new Information.DungeonInfo(7200, 1800);
                    }
                    pendingFloorInfo = new Information.FloorInfo(VoidRiftModWorld.currentDungeon.floorInfos.Count, "Void rift " + VoidRiftModWorld.currentDungeon.floorInfos.Count + 1);
                }),
                new PassLegacy("Determining Room Position", progress =>
                {
#if DEBUG
                    ROIMod.instance.Logger.Debug("Determining room position");
#endif
                    progress.Message = "Determining Room Position - Running Dradon Gen Algorithm"; //Sets the text above the worldgen progress bar
                    pendingFloorInfo.algorithmResult = new DradonGenAlgorithm().SetMinDistance(300).SetMaxDistance(400).SetMinJump(200).SetMaxJump(400).SetStartingPoint(new Point(200, 200)).SetCanvaWidth(6800).SetCanvaHeight(1600);
                    pendingFloorInfo.GenerateLayout();
                }),

                new PassLegacy("Generating Obligatory Room", progress =>
                {
#if DEBUG
                    ROIMod.instance.Logger.Debug("First room generation phase");
#endif
                    progress.Message = "Generating Obligatory Room";

                    Information.RoomInfo newRoomInformation = new Information.RoomInfo();

                    newRoomInformation.roomX = pendingFloorInfo.startingRoomLocation.X;
                    newRoomInformation.roomY = pendingFloorInfo.startingRoomLocation.X;
                    newRoomInformation.roomWidth = startingRoomSize.X;
                    newRoomInformation.roomHeight = startingRoomSize.Y;
                    newRoomInformation.StartingRoom = true;

                    pendingFloorInfo.roomList.Add(newRoomInformation);

                    for(int i = 1; i < pendingFloorInfo.algorithmResult.mainLineList.Count - 1; i++)
                    {
                        newRoomInformation = new Information.RoomInfo();

                        newRoomInformation.roomX = pendingFloorInfo.algorithmResult.mainLineList[i].X;
                        newRoomInformation.roomY = pendingFloorInfo.algorithmResult.mainLineList[i].Y;

                        Point roomSize = WorldGen.genRand.Next(possibleNormalRoomSize);
                        newRoomInformation.roomWidth = roomSize.X;
                        newRoomInformation.roomHeight = roomSize.Y;

                        pendingFloorInfo.roomList.Add(newRoomInformation);
                    }

                    newRoomInformation = new Information.RoomInfo();

                    newRoomInformation.roomX = pendingFloorInfo.startingRoomLocation.X;
                    newRoomInformation.roomY = pendingFloorInfo.startingRoomLocation.X;

                    Point bossRoomSize = WorldGen.genRand.Next(possibleBossSizeRoom);

                    newRoomInformation.roomWidth = bossRoomSize.X;
                    newRoomInformation.roomHeight = bossRoomSize.Y;
                    newRoomInformation.BossRoom = true;
                    pendingFloorInfo.roomList.Add(newRoomInformation);
                }),
                
                new PassLegacy("Generating room", progress =>
                {
#if DEBUG
                    ROIMod.instance.Logger.Debug("Second room generation phase");
#endif
                    progress.Message = "Generating room";
                    foreach (Information.RoomInfo roomInfo in pendingFloorInfo.roomList)
                    {
                        for (int i = roomInfo.roomTopLeftCorner.X; i < roomInfo.roomTopLeftCorner.X + roomInfo.roomWidth; i++)
                        {
                            for (int j = roomInfo.roomTopLeftCorner.Y; j < roomInfo.roomTopLeftCorner.Y + roomInfo.roomHeight; j++)
                            {
                                Main.tile[i, j].active(false);
                            }
                        }
                    }
                }),
                new PassLegacy("Setting spawn point", progress =>
                {
#if DEBUG
                    ROIMod.instance.Logger.Debug("Setting player spawn point");
#endif
                    progress.Message = "Setting player spawn";

                    Main.spawnTileX = pendingFloorInfo.startingRoomLocation.X;
                    Main.spawnTileY = pendingFloorInfo.startingRoomLocation.Y;
                }),
                new PassLegacy("Finalize", progress =>
                {
#if DEBUG
                    ROIMod.instance.Logger.Debug("Finalizing dungeon rift generation");
#endif
                    progress.Message = "Finalizing the rift";

                    VoidRiftModWorld.currentFloorInfo = pendingFloorInfo;

                })

            };
        }

        public override void Load()
        {
            Main.dayTime = false;
            Main.time = 0;

            if (!VoidRiftModWorld.InVoidRift)
            {
                VoidRiftModWorld.InVoidRift = true;
                //VoidRiftModWorld.currentFloorInfo = new Information.FloorInfo(0, "Void rift Floor 1");
            }
        }

        public override void Unload()
        {
            if(!VoidRiftModWorld.continueDungeon)
                VoidRiftModWorld.InVoidRift = false;

            VoidRiftModWorld.currentDungeon.floorInfos.Add(VoidRiftModWorld.currentFloorInfo);
        }
    }
}
