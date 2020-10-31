using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    internal class WastelandRockWall : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.scale *= 0.5f;
            item.maxStack = 999;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.consumable = true;
            item.createWall = ModContent.WallType<Walls.WastelandRockWallSafe>();
        }

        public override void AddRecipes() => new RecipeBuilder(this, 4)
                .AddIngredient<WastelandRock>()
                .AddTile(TileID.WorkBenches)
                .Register();
    }
}