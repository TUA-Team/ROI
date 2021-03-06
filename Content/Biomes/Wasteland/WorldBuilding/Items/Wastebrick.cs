﻿using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    public class Wastebrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wastebrick");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.consumable = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = true;
            item.createTile = ModContent.TileType<WastelandBrick>();
        }
    }
}
