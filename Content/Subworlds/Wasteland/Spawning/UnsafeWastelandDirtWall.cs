using ROI.Content.Subworlds.Wasteland.WorldBuilding.Walls;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Spawning
{
    public sealed class UnsafeWastelandDirtWall : Commons.Spawning.SpawnCondition
    {
        public override bool Active(int x, int y) =>
            Main.tile[x, y].wall == ModContent.WallType<WastelandDirtWall>() && y > Main.maxTilesY - 200;


        public override float SpawnChance => 0.75f;
    }
}