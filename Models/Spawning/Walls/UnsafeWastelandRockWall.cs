using ROI.Walls.Wasteland;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Models.Spawning.Wasteland.Walls
{
    public class UnsafeWastelandRockWall : SpawnCondition
    {
        public override bool Active(int x, int y) =>
            Main.tile[x, y].wall == ModContent.WallType<WastelandRockWall>() && y > Main.maxTilesY - 200;


        public override float SpawnChance => 0.75f;
    }
}