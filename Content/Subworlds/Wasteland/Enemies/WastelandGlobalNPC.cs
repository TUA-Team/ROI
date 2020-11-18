using SubworldLibrary;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Enemies
{
    internal sealed class WastelandGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (!Subworld.IsActive<WastelandDepthSubworld>())
                return;

            pool.Clear();

            pool.Add(ModContent.NPCType<MutatedDemon>(), 3);
        }
    }
}
