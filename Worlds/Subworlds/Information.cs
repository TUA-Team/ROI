using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROI.Worlds.Subworlds
{
    class Information
    {
        /// <summary>
        /// Will be mainly used for the overlay
        /// </summary>
        internal class FloorInfo
        {
            public string floorName;

            public int dungeonLevel;
            public int score;
            public int kill;

            public List<RoomInfo> roomList = new List<RoomInfo>();

            public bool completed; 

            public TimeSpan timeSpent;

            public DateTime startedTime; //Generally refer to system timespan

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

            public void Update()
            {
                timeSpent = DateTime.Now - startedTime;
            }
        }

        class DungeonInfo
        {
            public List<FloorInfo> floorInfos;

            public FloorInfo currentFloor;

            public int dungeonWidth, dungeonHeight;
            public int totalScore;

            public TimeSpan totalTimeSpent;

            public void Update()
            {
                if (currentFloor != null)
                {
                    currentFloor.Update();
                }
            }
        }


        internal class RoomInfo
        {
            public bool cleared;

            public int amountOfEnemy;
            public int roomWidth, roomHeight;

        }
    }
}
