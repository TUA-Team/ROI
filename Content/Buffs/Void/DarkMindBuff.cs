using Terraria;

namespace ROI.Content.Buffs.Void
{
    public class DarkMindBuff : ROIBuff
    {
        public DarkMindBuff() : base("Dark Mind", "-50% less damamage\n+50% debuff duration", persistent: true, canBeCleared: false, longerExpertDebuff: true)
        {
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // TODO: something for this
            /*ROIPlayer roiPlayer = ROIPlayer.Get(player);

            player.allDamageMult -= 0.5f;
            roiPlayer.DebuffDurationMultiplier += 0.5f;*/
        }
    }
}
