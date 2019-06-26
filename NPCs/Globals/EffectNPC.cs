using Terraria;
using Terraria.ModLoader;

namespace ROI.NPCs.Globals
{
    public class EffectNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        internal byte fireflyStunned;

        public override void ResetEffects(NPC npc)
        {
            fireflyStunned = 0;
        }

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (fireflyStunned != 0)
            {
                fireflyStunned--;
                npc.velocity *= .8f;
            }
            return true;
        }
    }
}
