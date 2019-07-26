using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items
{
    public abstract class ROIItem : ModItem
    {
        private readonly string _displayName, _tooltip;
        private readonly int _width, _height;

        protected ROIItem(string displayName, string tooltip, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White)
        {
            _displayName = displayName;
            _tooltip = tooltip;

            _width = width;
            _height = height;

            Value = value;
            Defense = defense;
            Rarity = rarity;
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault(_displayName);
            Tooltip.SetDefault(_tooltip);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.width = _width;
            item.height = _height;

            item.value = Value;
            item.defense = Defense;
            item.rare = Rarity;
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //Tooltip.SetDefault(_tooltip);

            PostModifyTooltips(tooltips);
            base.ModifyTooltips(tooltips);
        }

        public virtual void PostModifyTooltips(List<TooltipLine> tooltips)
        {
        }


        public int Value { get; }
        public int Defense { get; }
        public int Rarity { get; }
    }
}
