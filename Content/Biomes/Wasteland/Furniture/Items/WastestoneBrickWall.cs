using Terraria;
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
            Item.width = 32;
            Item.height = 32;
            Item.placeStyle = 1;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useTime = 7;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.createWall = ModContent.WallType<Walls.WastestoneBrickWall>();
        }
    }
}
