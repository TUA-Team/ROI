using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.Furniture.Items
{
    public sealed class WastebrickDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wastebrick Door");
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
            item.createTile = ModContent.TileType<Tiles.WastebrickDoorClosed>();
        }
    }
}
