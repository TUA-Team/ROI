using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Irrawood
{
    [AutoloadEquip(EquipType.Legs)]
    class IrrawoodLeggings : IrrawoodArmor
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irrawood Greaves");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.defense = 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.Sawmill);
            recipe.AddIngredient(mod.ItemType("Irrawood"), 35);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
