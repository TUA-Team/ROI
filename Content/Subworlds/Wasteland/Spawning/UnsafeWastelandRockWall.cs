using ROI.Content.Subworlds.Wasteland.WorldBuilding.Walls;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Spawning
{
    internal sealed class UnsafeWastelandRockWall : WastelandSpawnCondition
    {
        public override bool Active(Player player, int x, int y) =>
            base.Active(player, x, y) && Main.tile[x, y].wall == ModContent.WallType<WastelandRockWall>();


        public override float SpawnChance => 0.75f;
    }
}