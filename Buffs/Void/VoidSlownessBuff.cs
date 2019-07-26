using Terraria;

namespace ROI.Buffs.Void
{
    public sealed class VoidSlownessBuff : ROIBuff
    {
        public VoidSlownessBuff() : base("Void Slowness", "", persistent: true, canBeCleared: false, longerExpertDebuff: true)
        {
        }

        public override void Update(Player player, ref int buffIndex) => player.moveSpeed *= 0.75f;
    }
}
