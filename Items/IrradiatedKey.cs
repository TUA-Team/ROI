using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items
{
    class IrradiatedKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irradiated Key");
            Tooltip.SetDefault("Allow to open Irradiated chest");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 20;
            item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.Hellforge);
            recipe.AddIngredient(ItemID.ShadowKey);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
