using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items.Placeables.Wasteland
{
    internal class IrradiatedChest : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.Swing;
            item.consumable = true;
            item.value = 500;
            item.createTile = ModContent.TileType<Tiles.Wasteland.IrradiatedChest>();
        }
    }
}