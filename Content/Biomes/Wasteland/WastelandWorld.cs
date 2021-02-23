using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Content.Biomes.Wasteland
{
    internal sealed class WastelandWorld : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            var index = tasks.FindIndex(x => x.Name.Equals("Granite"));

            if (index != -1)
            {
                tasks.Insert(index + 1, new PassLegacy("ROI: Wasteland", new WastelandWorldMaker(mod).Make));
            }
        }
    }
}
