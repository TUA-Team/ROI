using SubworldLibrary;
using Terraria;

namespace ROI.Content.Subworlds.Wasteland.Spawning
{
    internal abstract class WastelandSpawnCondition : Commons.Spawning.SpawnCondition
    {
        public override bool Active(Player player, int x, int y) =>
            Subworld.IsActive<WastelandDepthSubworld>();
    }
}
