using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding.Items
{
    public sealed class WastelandWaste : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solidified Waste");
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
            item.useStyle = 1;
            item.autoReuse = true;
            item.createTile = ModContent.TileType<Tiles.WastelandWaste>();
        }
    }
}
