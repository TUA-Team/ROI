using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Items.Placeables.Wasteland
{
    internal class WastestoneBrickWall : ModItem
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
            item.useTime = 1;
            item.useStyle = 1;
            item.consumable = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.maxStack = 999;
            item.createWall = ModContent.WallType<Walls.Wasteland.WastestoneBrickWall>();
        }
    }
}
