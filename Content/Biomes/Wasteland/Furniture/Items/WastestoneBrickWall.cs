﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.Furniture.Items
{
    public class WastestoneBrickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wastestone Brick Wall");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.placeStyle = 1;
            item.value = Item.sellPrice(0, 0, 0, 0);
            item.useTime = 7;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.maxStack = 999;
            item.createWall = ModContent.WallType<Walls.WastestoneBrickWall>();
        }
    }
}
