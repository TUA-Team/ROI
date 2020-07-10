using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Placeables.Wasteland
{
    class Wasteland_Rock_Wall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irradiated rock wall");
            Tooltip.SetDefault("Won't spawn any mob");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.scale *= 0.5f;
            item.maxStack = 999;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.useTime = 10;
            item.consumable = true;
            item.createWall = mod.WallType("Wasteland_Rock_Wall");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("Wasteland_Rock"), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}
