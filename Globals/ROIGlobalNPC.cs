using Terraria;
using Terraria.ModLoader;

namespace ROI.Globals
{
    internal partial class ROIGlobalNPC : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            VoidSetDefaults(npc);
            EffectSetDefaults();
        }

        public override bool PreNPCLoot(NPC npc)
        {
            bool flag = false;
            flag |= VoidPreNPCLoot(npc);
            return flag;
        }

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            bool flag = false;
            flag |= EffectStrikeNPC(npc);
            return flag;
        }
    }
}
