using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Dusts
{
	abstract class TeleportationDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			
		}

		public override bool Update(Dust dust)
		{
			return base.Update(dust);
		}

	}
}
