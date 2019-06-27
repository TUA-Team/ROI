using Terraria;

namespace ROI.Globals
{
    internal partial class ROIGlobalNPC
    {
        internal byte fireflyStunned;

        private void EffectSetDefaults(NPC npc)
        {
            fireflyStunned = 0;
        }

        private bool EffectStrikeNPC(NPC npc)
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
