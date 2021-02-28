namespace ROI.Content.Buffs.Void
{
    public class PillarPresence : ROIBuff
    {
        public PillarPresence() : base("Pillar Presence",
            "You sense something un-earthly", persistent: true, canBeCleared: false, debuff: true)
        {
        }
    }
}