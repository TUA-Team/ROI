using Terraria.ModLoader;

namespace ROI.Buffs
{
    internal class ROIBuff : ModBuff
    {
        private readonly string _displayName, _description;

        protected ROIBuff(string displayName) : this(displayName, "")
        {
        }

        protected ROIBuff(string displayName, string description)
        {
            _displayName = displayName;
            _description = description;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            DisplayName.SetDefault(_displayName);
            Description.SetDefault(_description);
        }
    }
}
