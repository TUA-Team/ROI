using Terraria;

namespace ROI.Content.Buffs.Void
{
    public class VoidSlownessBuff : ROIBuff
    {
        public VoidSlownessBuff() : base("Void Slowness", "You are being consumed by the void\n-75% movement speed", persistent: true, canBeCleared: false, debuff: true, longerExpertDebuff: true)
        {
        }

        public override void Update(Player player, ref int buffIndex) => player.moveSpeed *= 1 - 0.75f;
    }
}
