using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Worlds.Subworlds
{
    class VoidRiftModWorld : ModWorld
    {
        public static bool InVoidRift { get; set; }

        internal static Information.DungeonInfo currentDungeon;
        internal static Information.FloorInfo currentFloorInfo;

        internal static bool continueDungeon;

        public override void PreUpdate()
        {
            if (InVoidRift)
                currentFloorInfo?.Update();
            base.PreUpdate();
        }

        public override void PostDrawTiles()
        {
            if (!InVoidRift) return;
            Point startingRoom = currentFloorInfo.startingRoomLocation;
            if (currentFloorInfo.dungeonLevel == 0 && currentFloorInfo.timeSpent < new TimeSpan(0, 0 , 5, 0))
            {
                Main.spriteBatch.Begin();
                string welcomeText = "Welcome to the void rift!\n" +
                                     "In this dungeon, your skill will be tested to the extreme.\n" +
                                     "Fight strong monster, loot special room and come back with everything or nothing!\n" +
                                     "Do you think you have what it take to take over the void rift and destroy the original pillar?\n" +
                                     "Go in the next room to start your adventure";


                Vector2 textSize = Main.fontDeathText.MeasureString(welcomeText);
                Point startingRoomLocation = new Point(currentFloorInfo.roomList[0].roomX, currentFloorInfo.roomList[0].roomY);

                Vector2 textLocation = new Vector2(startingRoomLocation.X * 16 - textSize.X / 2, startingRoomLocation.Y * 16 - textSize.Y / 2);

                Utils.DrawBorderString(Main.spriteBatch, welcomeText, textLocation - Main.screenPosition, Color.White);
                Main.spriteBatch.End();
            }
            base.PostDrawTiles();
        }
    }
}
