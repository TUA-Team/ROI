using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ROI.Items.Placeable.Wasteland
{
    class WastelandView : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nuclear catastrophe");
            Tooltip.SetDefault("\"Dradon\"");
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 48;
            item.height = 32;
            item.useTime = 1;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.createTile = mod.TileType("WastelandView");
        }
    }
}
