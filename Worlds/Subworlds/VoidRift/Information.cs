using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bubble_Defender.Algorithm;
using Microsoft.Xna.Framework;

namespace ROI.Worlds.Subworlds
{
    public class Information
    {
        /// <summary>
        /// Will be mainly used for the overlay
        /// </summary>
        public class FloorInfo
        {
            public string floorName;

            public int dungeonLevel;
            public int score;
            public int kill;

            public DradonGenAlgorithm algorithmResult;

            public List<RoomInfo> roomList = new List<RoomInfo>();

            public bool completed; 

            public TimeSpan timeSpent;

            public DateTime startedTime; //Generally refer to system timespan

            public Point startingRoomLocation;
            public Point bossRoomLocation;

            public FloorInfo(int dungeonLevel, string floorName)
            {
                this.dungeonLevel = dungeonLevel;
                this.score = 0;
                this.kill = 0;
                this.completed = false;
                this.floorName = floorName;

                

                timeSpent = TimeSpan.Zero;
                 
                startedTime = DateTime.Now;
            }

            public void GenerateLayout()
            {
                algorithmResult.GenerateMainBranch();
                startingRoomLocation = algorithmResult.startingPoint;
                bossRoomLocation = algorithmResult.endingPoint;
            }

            public void Update()
            {
                timeSpent = DateTime.Now - startedTime;
            }
        }

        internal class DungeonInfo
        {
            public List<FloorInfo> floorInfos;

            public FloorInfo currentFloor;

            public int dungeonWidth, dungeonHeight;
            public int totalScore;//To be implemented

            public TimeSpan totalTimeSpent;

            public DungeonInfo(int dungeonWidth, int dungeonHeight)
            {
                floorInfos = new List<FloorInfo>();
                this.dungeonWidth = dungeonWidth;
                this.dungeonHeight = dungeonHeight;
            }

            public void Update()
            {
                if (currentFloor != null)
                {
                    currentFloor.Update();
                }
            }

            public void UpdateNewFloorInfo()
            {
                totalTimeSpent += currentFloor.timeSpent;
            }
        }


        public class RoomInfo
        {
            public bool cleared;

            public bool special;

            private bool _startingRoom;
            private bool _bossRoom;

            public bool StartingRoom
            {
                get => _startingRoom;
                set
                {
                    if (special)
                        return;
                    special = true;
                    _startingRoom = true;
                }
            }

            public bool BossRoom
            {
                get => _bossRoom;
                set
                {
                    if (special)
                        return;
                    special = true;
                    _bossRoom = true;
                }
            }


            public int amountOfEnemy;
            public int roomWidth, roomHeight, roomX, roomY;
            public Point roomTopLeftCorner => new Point(roomX - roomWidth / 2, roomY - roomHeight / 2);

            public Rectangle roomBound => new Rectangle(roomTopLeftCorner.X, roomTopLeftCorner.Y, roomWidth, roomHeight);
        }
    }
}
