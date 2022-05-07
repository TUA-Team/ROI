using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Buffs
{
    public abstract class ROIBuff : ModBuff
    {
        private readonly string _displayName, _tooltip;
        private readonly bool _hideTime, _save, _persistent, _canBeCleared, _debuff, _longerExpertDebuff;

        protected ROIBuff(string displayName, string tooltip, bool hideTime = false, bool save = false, bool persistent = false, bool canBeCleared = true, bool debuff = false, bool longerExpertDebuff = false)
        {
            _displayName = displayName;
            _tooltip = tooltip;

            _hideTime = hideTime;
            _save = save;
            _persistent = persistent;
            _canBeCleared = canBeCleared;

            _debuff = debuff;
            _longerExpertDebuff = longerExpertDebuff;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(_displayName);
            Description.SetDefault(_tooltip);

            Main.buffNoTimeDisplay[Type] = _hideTime;
            Main.buffNoSave[Type] = !_save;
            Main.persistentBuff[Type] = _persistent;

            Main.debuff[Type] = _debuff;
            // TODO: ?? LongerExpertDebuff = _longerExpertDebuff;
        }
    }
}