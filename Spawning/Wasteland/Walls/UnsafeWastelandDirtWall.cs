using Terraria;

namespace ROI.Spawning.Wasteland.Walls
{
    public class UnsafeWastelandDirtWall : SpawnCondition
    {
        public override bool Active(int x, int y) =>
            Main.tile[x, y].wall == ROIMod.Instance.WallType<WastelandDirtWall>() && y > Main.maxTilesY - 200;


        public override float SpawnChance => 0.75f;
    }
}