using Terraria;

namespace ROI.Buffs.Void
{
    public sealed class PillarPresenceBuff : ROIBuff
    {
        public PillarPresenceBuff() : base("Pillar Presence", "You sense something un-earthly", persistent: true, canBeCleared: false, debuff: true)
        {
        }
    }
}