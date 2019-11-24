using Terraria.ModLoader;

namespace ROI.Items.Placeables.Wasteland
{
    class WastelandDirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irradiated Dirt");
            Tooltip.SetDefault("\"Is it even safe to touch it?\"");
        }

        public override void SetDefaults()
        {
            item.maxStack = 9999;
            item.consumable = true;
            item.width = 16;
            item.height = 16;
            item.useAnimation = 15;
            item.autoReuse = true;
            item.useTime = 10;
            item.useStyle = 1;
            item.useTurn = true;
            item.createTile = ModContent.TileType<Tiles.Wasteland.WastelandDirt>();
        }
    }
}
