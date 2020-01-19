using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
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
                int count = tasks.RemoveAll(x => x.Name == "Underworld");
                if (count != 1) mod.Logger.WarnFormat("{0} World Gen Tasks named \"Underworld\"", count);
                count = tasks.RemoveAll(x => x.Name == "Hellforge");
                if (count != 1) mod.Logger.WarnFormat("{0} World Gen Tasks named \"Hellforge\"", count);
                if (index != -1)
                {
                    tasks.Insert(index + 1, new PassLegacy("ROI: Wasteland", WastelandGeneration));
                }
            }
        }
    }
}