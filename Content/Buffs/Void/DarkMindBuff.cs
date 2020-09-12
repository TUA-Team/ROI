using ROI.Content.Buffs;
using ROI.Players;
using Terraria;

namespace ROI.Content.Buffs.Void
{
    public sealed class DarkMindBuff : ROIBuff
    {
        public DarkMindBuff() : base("Dark Mind", "-50% less damamage\n+50% debuff duration", persistent: true, canBeCleared: false, longerExpertDebuff: true)
        {
        }

        public override void Update(Player player, ref int buffIndex)
        {
            ROIPlayer roiPlayer = ROIPlayer.Get(player);

            roiPlayer.player.allDamageMult -= 0.5f;
            roiPlayer.DebuffDurationMultiplier += 0.5f;
        }
    }
}
