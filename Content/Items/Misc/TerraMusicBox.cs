using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items.Misc
{
    public class TerraMusicBox : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.Swing;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = ModContent.TileType<Content.Tiles.TerraMusicBox>();
            item.width = 30;
            item.height = 10;
            item.rare = ItemRarityID.LightRed;
            item.value = 100000;
            item.accessory = true;
        }
    }
}