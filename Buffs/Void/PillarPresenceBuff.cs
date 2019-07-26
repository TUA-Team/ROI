using Terraria;

namespace ROI.Buffs.Void
{
    public sealed class PillarPresenceBuff : ROIBuff
    {
        public PillarPresenceBuff() : base("Pillar Presence", "", persistent: true, canBeCleared: false)
        {
        }
    }
}