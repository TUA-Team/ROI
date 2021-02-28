using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    public class Irrawood : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 9999;
            item.consumable = true;
            item.width = 24;
            item.height = 22;
            item.useAnimation = 15;
            item.autoReuse = true;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            //item.createTile = ModContent.TileType("Irrawood");
        }
    }
}