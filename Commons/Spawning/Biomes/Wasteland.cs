using Terraria;

namespace ROI.Commons.Spawning.Biomes
{
    class Wasteland : SpawnCondition
    {
        protected override float SpawnChance => 1.0f;

        protected override bool Active(int x, int y) => Main.ActiveWorldFileData.HasCrimson && y > Main.maxTilesY - 200 ? true : false;
    }
}
