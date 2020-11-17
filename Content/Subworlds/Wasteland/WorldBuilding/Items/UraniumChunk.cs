using ROI.Content.Subworlds.Wasteland.WorldBuilding.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding.Items
{
    internal class UraniumChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Uranium chunk");
            Tooltip.SetDefault("Shiny green radioactivity.\nWill be used in the 0.2 update - The nuclear and reforge rework update");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 16;
            item.height = 16;
            item.useAnimation = 15;
            item.autoReuse = true;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.createTile = ModContent.TileType<UraniumOre>();
        }
    }
}
