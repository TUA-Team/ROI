using Terraria;
using Terraria.ModLoader;

namespace ROI.Buffs.Void
{
    class PillarPresence : ModBuff
    {
        public override void SetDefaults()
        {
            canBeCleared = false;
            longerExpertDebuff = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            ROIWorld world = mod.GetModWorld<ROIWorld>();
            if (world.StrangePresenceDebuff)
            {
                world.StrangePresenceDebuff = true;
            }
        }
    }
}
