using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.NPCs
{
    public sealed partial class MainGlobalNPC : GlobalNPC
    {
        public override bool PreNPCLoot(NPC npc)
        {
            /*if (!PreNPCLootVoid(npc)) return false;

            return true;*/

            return true;
        }

        public override void SetDefaults(NPC npc)
        {
            //SetDefaultsVoid(npc);
        }


        //public override bool InstancePerEntity => true;
    }
}
