using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Enemies
{
    public sealed class MutatedDemon : ModNPC
    {
        public override string Texture => "Terraria/NPC_" + NPCID.VoodooDemon;

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.VoodooDemon);
        }
    }
}
