using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Irradiated
{
	[AutoloadEquip(EquipType.Body)]
	class IrradiatedChestplate : ArmorBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Irradiated Chestplate");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.defense = 8;
		}



		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Hellforge);
			recipe.AddIngredient(mod.ItemType("Irrawood"), 40);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
