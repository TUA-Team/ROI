using System.Collections.Generic;
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
        public override int width => 1000;
        public override int height => 1000;
        public override List<GenPass> tasks => GeneratePassList();

        public override ModWorld modWorld => ModContent.GetInstance<VoidRiftModWorld>();

        public override bool saveSubworld => false;
        public override bool disablePlayerSaving => true;


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
                            Main.tile[i, j].type = TileID.Stone;

                            
                        }
                    }
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
                VoidRiftModWorld.currentFloorInfo = new VoidRiftModWorld.FloorInfo(0, "Void rift Floor 1");
            }
        }

        public override void Unload()
        {
            VoidRiftModWorld.InVoidRift = false;
        }
    }
}
