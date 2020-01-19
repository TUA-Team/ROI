using Terraria.ModLoader;

namespace ROI.Items.Misc
{
    public class TerraMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Realms of Inf Terra)");
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.TerraMusicBox>();
            item.width = 30;
            item.height = 10;
            item.rare = 4;
            item.value = 100000;
            item.accessory = true;
        }
    }
}