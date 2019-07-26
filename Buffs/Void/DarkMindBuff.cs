using ROI.Players;
using Terraria;

namespace ROI.Buffs.Void
{
    public sealed class DarkMindBuff : ROIBuff
    {
        public DarkMindBuff() : base("Dark Mind", "", persistent: true, canBeCleared: false, longerExpertDebuff: true)
        {
        }

        public override void Update(Player player, ref int buffIndex) => ROIPlayer.Get(player).DebuffDurationMultiplier += 0.5f;
    }
}
