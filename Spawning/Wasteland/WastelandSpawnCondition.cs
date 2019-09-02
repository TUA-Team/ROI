using Terraria;

namespace ROI.Spawning.Wasteland
{
    public class WastelandSpawnCondition : SpawnCondition
    {
        public override bool Active(int x, int y) =>
            Main.ActiveWorldFileData.HasCrimson && y > Main.maxTilesY - 200;


        public override float SpawnChance => 1;
    }
}