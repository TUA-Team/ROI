using ROI.Content.Subworlds.Wasteland.WorldBuilding.Walls;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Spawning
{
    internal sealed class UnsafeWastelandDirtWall : WastelandSpawnCondition
    {
        public override bool Active(Player player, int x, int y) =>
            base.Active(player, x, y) && Main.tile[x, y].wall == ModContent.WallType<WastelandDirtWall>();


        public override float SpawnChance => 0.75f;
    }
}