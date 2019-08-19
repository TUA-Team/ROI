﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ROI.Items.Placeable.Wasteland
{
    class Irrawood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irrawood");
            Tooltip.SetDefault("It glow from the inside");
        }

        public override void SetDefaults()
        {
            item.maxStack = 9999;
            item.consumable = true;
            item.width = 24;
            item.height = 22;
            item.useAnimation = 15;
            item.autoReuse = true;
            item.useTime = 10;
            item.useStyle = 1;
            item.useTurn = true;
            //item.createTile = mod.TileType("Irrawood");
        }
    }
}
