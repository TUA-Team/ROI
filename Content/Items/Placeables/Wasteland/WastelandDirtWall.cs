using ROI.Content.Walls.Wasteland;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items.Placeables.Wasteland
{
    internal class WastelandDirtWall : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.scale *= 0.5f;
            item.maxStack = 999;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.Swing;
            item.useTime = 10;
            item.consumable = true;
            item.createWall = ModContent.WallType<WastelandDirtWallSafe>();
        }

        public override void AddRecipes() => CreateRecipe(4)
                .AddIngredient<WastelandDirt>()
                .AddTile(TileID.WorkBenches)
                .Register();
    }
}