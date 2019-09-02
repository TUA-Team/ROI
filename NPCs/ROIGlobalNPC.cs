using Terraria;
using Terraria.ModLoader;

namespace ROI.NPCs
{
    public sealed partial class ROIGlobalNPC : GlobalNPC
    {
        public override bool PreNPCLoot(NPC npc)
        {
            if (!PreNPCLootVoid(npc)) return false;

            return true;
        }

        public override void SetDefaults(NPC npc)
        {
            SetDefaultsVoid(npc);
        }


        public override bool InstancePerEntity => true;
    }
}
