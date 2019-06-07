using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ROI.Items
{
	/// <summary>
	/// Base of any RoI item
	/// </summary>
	internal abstract class ROIItem : ModItem
	{
		/// <summary>
		/// Override this to set it true in case it's a void item
		/// </summary>
		public virtual bool Void { get; }

		/// <summary>
		/// We sealed this one because we wanna make sure everything is set if it's a void item or another custom class item
		/// </summary>
		public sealed override void SetDefaults()
		{
			TrueSetDefaults();
			if (Void)
			{
				item.summon = false;
				item.ranged = false;
				item.melee = false;
				item.thrown = false;
				item.magic = false;
			}
		}
		/// <summary>
		/// Use this SetDefaults just in case it's a void item
		/// </summary>
		public virtual void TrueSetDefaults()
		{

		}
	}
}
