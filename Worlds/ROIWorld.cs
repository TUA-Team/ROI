using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace ROI.Worlds
{
    internal sealed partial class ROIWorld : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            if (ROIMod.DebugConfig.GenWasteland)
            {
                int index = tasks.FindIndex(x => x.Name == "Underworld");
                tasks.RemoveAll(x => x.Name == "Underworld");
                tasks.RemoveAll(x => x.Name == "Hellforge");
                if (index != -1)
                {
                    tasks.Insert(index + 1, new PassLegacy("ROI: Wasteland", WastelandGeneration));
                }
            }
        }
    }
}