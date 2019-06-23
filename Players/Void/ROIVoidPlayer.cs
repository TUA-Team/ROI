using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Players
{
	public sealed partial class ROIPlayer : ModPlayer
	{
		/// <summary>
		/// 1 minute cooldown
		/// </summary>
		public int voidEffectAttemptCooldown = 60 * 60;

		public int voidItemCooldown = 60 * 60 * 5;

		internal int VoidAffinityAmount
		{
			get => _voidAffinityAmount;
		}


		public int AddVoidAffinity(int voidAffinity, bool simulate = false)
		{
			int simulatedAmount = Math.Min(MaxVoidAffinity - VoidAffinityAmount, voidAffinity);
			if (!simulate)
			{
				_voidAffinityAmount += simulatedAmount;
			}
			return simulatedAmount;
		}

		public void DamageVoidHeart(ref int damage)
		{

			if (VoidHeartHP >= 0)
			{
			    CombatText.NewText(
			        new Rectangle((int) player.position.X, (int) player.position.Y, player.width, player.height),
			        Color.Black, damage, true, true);
				VoidHeartHP -= damage;
				damage = 0;
			}
		}
	}
}
