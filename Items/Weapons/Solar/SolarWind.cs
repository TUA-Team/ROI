using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ROI.Items.Weapons.Solar
{
	class SolarWind : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar wind");
			Tooltip.SetDefault("May the solar wind be with you!\n" +
			                   "This item cannot be reforged");
		}

		public override void SetDefaults()
		{
			item.magic = true;
			item.mana = 100;
			item.damage = 150;
			
			base.SetDefaults();
		}

		public override bool NewPreReforge()
		{
			return false;
		}
	}
}
