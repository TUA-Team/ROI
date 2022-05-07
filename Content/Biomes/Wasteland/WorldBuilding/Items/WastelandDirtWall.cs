using ROI.Content.Biomes.Wasteland.WorldBuilding.Walls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    public class WastelandDirtWall : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.scale *= 0.5f;
            Item.maxStack = 999;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<WastelandDirtWallSafe>();
        }

        public override void AddRecipes() => CreateRecipe(4)
                .AddIngredient<WastelandDirt>()
                .AddTile(TileID.WorkBenches)
                .Register();
    }
}