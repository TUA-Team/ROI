namespace ROI.Buffs.Void
{
    public sealed class PillarPresence : ROIBuff
    {
        public PillarPresence() : base("Pillar Presence",
            "You sense something un-earthly", persistent: true, canBeCleared: false, debuff: true)
        {
        }
    }
}