using Terraria.ModLoader;

namespace ROI.Items.Placeable.Wasteland
{
    class Wasteland_Ore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poisonstone");
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
            item.createTile = mod.TileType("Wasteland_Ore");
        }
    }
}
