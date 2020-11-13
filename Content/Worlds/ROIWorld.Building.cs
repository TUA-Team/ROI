using ROI.Content.Configs;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;

namespace ROI.Content.Worlds
{
    partial class ROIWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            if (!(mod.GetConfig("Debug") as DebugConfig).GenWasteland)
                return;

            int hellGen = tasks.FindIndex(i => i.Name == "Underworld");
            int hellForgeGen = tasks.FindIndex(i => i.Name == "Hellforge");
            //Mod.Logger.Debug("maxTilesX: " + Main.maxTilesX);
            //Mod.Logger.Debug("maxTilesY" + Main.maxTilesY);
            if (hellGen != -1)
            {
                tasks[hellGen] = new PassLegacy("Underworld", (progress) =>
                {
                    OriginalUnderworldGeneration(progress);
                });
            }

            if (hellForgeGen != -1)
            {
                /*tasks[hellForgeGen] = new PassLegacy("Hellforge", (progress) =>
                {
                    // Why the fuck is this it's own phase in the first place
                    // TODO: (very low prio)
                    return;
                });*/
            }
        }
    }
}
