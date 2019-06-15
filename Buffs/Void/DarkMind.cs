using ROI.Players;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Buffs.Void
{
    class DarkMind : ModBuff
    {
        public override void SetDefaults()
        {
            canBeCleared = false;
            longerExpertDebuff = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ROIPlayer>().darkMind = true;
        }
    }
}
