using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Tools.Wasteland
{
    class WastestoneWaraxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eradiated Waraxe");
            Tooltip.SetDefault("Holds the power of hundreds of fragmented souls.");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 38;
            item.axe = 12;
            item.hammer = 125;
            item.melee = true;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.damage = 35;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.crit = 10;
            item.useTime = 30;
            item.useAnimation = 15;
            item.knockBack = 0.7f;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Wasteland_Ingot"), 15);
            recipe.SetResult(this, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
        }

        // TODO: debuff
    }
}
