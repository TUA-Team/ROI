using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Furniture.Items
{
    class Wastebrick_Chest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wastebrick Chest");
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
            item.useStyle = 1;
            item.consumable = true;
            item.value = 500;
            item.createTile = ModContent.TileType<Tiles.WastebrickChest>();
        }
    }
}
