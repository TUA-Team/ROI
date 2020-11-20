using ROI.Commons.Spawning;
using Terraria;

namespace ROI.Content.Subworlds.Wasteland.Spawning
{
    public sealed class WastelandSpawnCondition : SpawnCondition
    {
        public override bool Active(int x, int y) =>
            Main.ActiveWorldFileData.HasCrimson && y > Main.maxTilesY - 200;


        public override float SpawnChance => 1;
    }
}