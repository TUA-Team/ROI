using Terraria.ModLoader;

namespace ROI.Items.Placeables.Furniture
{
    class Wastebrick_Door : ModItem
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
            item.createTile = mod.TileType("Wastebrick_Door_Closed");
        }
    }
}
