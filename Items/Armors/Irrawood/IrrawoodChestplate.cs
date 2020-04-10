using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Irrawood
{
    [AutoloadEquip(EquipType.Body)]
    class IrrawoodChestplate : IrrawoodArmor
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irrawood Chestplate");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.defense = 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.Sawmill);
            recipe.AddIngredient(mod.ItemType("Irrawood"), 40);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
