﻿using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ROI.Worlds.Subworlds
{
    class VoidRiftModWorld : ModWorld
    { 
        /// <summary>
        /// Will be mainly used for the overlay
        /// </summary>
        internal class FloorInfo
        {
            public int dungeonLevel;
            public int score;
            public int kill;
            public bool completed; 
            public TimeSpan timeSpent;

            public DateTime startedTime; //Generally refer to system timespan

            public FloorInfo(int dungeonLevel)
            {
                this.dungeonLevel = dungeonLevel;
                this.score = 0;
                this.kill = 0;
                this.completed = false;

                timeSpent = TimeSpan.Zero;
                 
                startedTime = DateTime.Now;
            }

            public void Update()
            {
                timeSpent = DateTime.Now - startedTime;
            }
        }

        public static bool InVoidRift { get; set; }

        internal static List<FloorInfo> previousFloorInfoList;
        internal static FloorInfo currentFloorInfo;

        public override void Initialize()
        {
            if (InVoidRift && currentFloorInfo != null)
            {
                previousFloorInfoList.Add(currentFloorInfo);
                currentFloorInfo = new FloorInfo(previousFloorInfoList[previousFloorInfoList.Count - 1].dungeonLevel + 1);
            }

            if (InVoidRift && currentFloorInfo == null)
            {
                currentFloorInfo = new FloorInfo(0);
            }
        }

        public override void PreUpdate()
        {
            if(InVoidRift)
                currentFloorInfo?.Update();
            base.PreUpdate();
        }
    }
}
