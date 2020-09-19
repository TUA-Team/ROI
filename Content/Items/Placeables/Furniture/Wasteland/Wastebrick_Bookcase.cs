﻿using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items.Placeables.Furniture.Wasteland
{
    internal class Wastebrick_Bookcase : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wastebrick Bookcase");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.Swing;
            item.consumable = true;
            item.value = 500;
            item.createTile = ModContent.TileType<Tiles.Furniture.Wasteland.WastebrickBookcase>();
        }
    }
}
