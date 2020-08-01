using Terraria;

namespace ROI.Commons.Spawning.Walls
{
    class Unsafe_Wasteland_Rock_Wall : SpawnCondition
    {
        protected override bool Active(int x, int y)
        {
            int wallType = Main.tile[x, y].wall;
            if (wallType == mod.WallType("Wasteland_Rock_Wall") && y > Main.maxTilesY - 200)
            {
                return true;
            }
            return false;
        }

        protected override float SpawnChance => 0.75f;
    }
}
