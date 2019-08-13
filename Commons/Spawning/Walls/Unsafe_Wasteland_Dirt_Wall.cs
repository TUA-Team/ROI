using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROI.Commons.Spawning.Walls
{
    class Unsafe_Wasteland_Dirt_Wall : SpawnCondition
    {
        protected override bool Active(int x, int y)
        {
            int wallType = Main.tile[x, y].wall;
            if (wallType == mod.WallType("Wasteland_Dirt_Wall") && y > Main.maxTilesY - 200)
            {
                return true;
            }
            return false;
        }

        protected override float SpawnChance => 0.75f;
    }
}
