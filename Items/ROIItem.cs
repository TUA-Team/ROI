using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items
{
    public abstract class ROIItem : ModItem
    {
        private readonly string _displayName, _tooltip;
        private readonly int _width, _height, _stack;

        protected ROIItem(string displayName, string tooltip, int width, int height, int value = 0, int defense = -1, int rarity = ItemRarityID.White, int stack = 1) {
            _displayName = displayName;
            _tooltip = tooltip;

            _width = width;
            _height = height;
            _stack = stack;

            Value = value;
            Defense = defense;
            Rarity = rarity;
        }


        public override void SetStaticDefaults() {
            DisplayName.SetDefault(_displayName);
            Tooltip.SetDefault(_tooltip);
        }

        public override void SetDefaults() {
            item.width = _width;
            item.height = _height;

            item.value = Value;
            item.defense = Defense;
            item.rare = Rarity;
        }


        public sealed override void ModifyTooltips(List<TooltipLine> tooltips) {
            //Tooltip.SetDefault(_tooltip);

            PostModifyTooltips(tooltips);
        }

        public virtual void PostModifyTooltips(List<TooltipLine> tooltips) {
        }


        public int Value { get; }
        public int Defense { get; }
        public int Rarity { get; }
    }
}
