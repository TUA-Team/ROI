using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Placeables.Wasteland
{
    class Wasteland_Forge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wasteland Forge");
            Tooltip.SetDefault("Used to transform Toxic Stone ore in bar");
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.useTime = 10;
            item.consumable = true;
            item.createTile = mod.WallType("Wasteland_Forge");
        }
    }
}
