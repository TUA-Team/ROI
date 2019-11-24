using System.Collections.Generic;
using Terraria.ModLoader;

namespace ROI.Globals
{
    class ROIGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<Players.ROIPlayer>().ZoneWasteland)
            {
                pool.Clear();
            }
        }
    }
}
