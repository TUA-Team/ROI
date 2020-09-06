using Terraria.ModLoader;
using Terraria.ID;

namespace ROI.Items.Misc
{
    public class TerraMusicBox : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.TerraMusicBox>();
            item.width = 30;
            item.height = 10;
            item.rare = ItemRarityID.LightRed;
            item.value = 100000;
            item.accessory = true;
        }
    }
}