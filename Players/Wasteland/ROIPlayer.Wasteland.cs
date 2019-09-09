using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Worlds;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.Players
{
	public sealed partial class ROIPlayer : ModPlayer
	{
		private int _radiationTimer = 60 * 2;
		private int _removeRadiationTimer = 60 * 20;
		

		internal void UpdateRadiation()
		{
			if (_radiationTimer <= 0 && WastelandCheck() && radiationLevel != 1f)
			{
				if (!ROIMod.instance.radiationInterface.IsVisible)
				{
					ROIMod.instance.radiationInterface.IsVisible = true;
				}
				radiationLevel += 0.01f;
				SetRadiationTimer();
			}
			else if(_removeRadiationTimer <= 0 && !WastelandCheck() && radiationLevel != 0.0f)
			{
				radiationLevel -= 0.01f;
				SetRadiationRemovalTimer();	
			}
			if (_radiationTimer >= 1)
			{
				_radiationTimer--;
			}

			if (_removeRadiationTimer >= 1)
			{
				_removeRadiationTimer--;
			}
			ApplyRadiationEffect();
		}

		internal bool WastelandCheck()
		{
			return Main.ActiveWorldFileData.HasCrimson && player.position.Y / 16 > Main.maxTilesY - 200 && (ROIMod.dev) || (mod.GetModWorld<ROIWorld>().version > new Version(0, 1));
		}

		internal void SetRadiationTimer()
		{
			_radiationTimer = 60 * 2;
			if (this.irrawoodSet)
			{
				_radiationTimer += 60 * 2;
			}
		}

		internal void SetRadiationRemovalTimer()
		{
			_removeRadiationTimer = 60 * 2;
		}

		internal void ApplyRadiationEffect()
		{
			if (radiationLevel >= 0.2)
			{
				player.statDefense /= 2;
			}

			if (radiationLevel >= 0.4 && radiationLevel < 0.7)
			{
				player.allDamage *= 0.75f;
			}

			if (radiationLevel >= 0.70)
			{
				player.statLifeMax2 = (int)(player.statLifeMax2 * 0.9f);
				player.lifeRegen = 0;
				player.allDamage *= 0.6f;
			}

			if (radiationLevel >= 0.85)
			{
				PlayerDeathReason reason = PlayerDeathReason.ByCustomReason(player.name + $" {GetDeathRadiationReason()}");
				player.Hurt(reason, 1, 0, false, true, false, 20);
			}
		}

		internal string GetDeathRadiationReason()
		{
			string[] deathReason =
			{
				"didn't check their radiation level",
				"tried to become hulk but failed at it",
				"didn't know how to survive to extreme radiation",
				"thought it was a good idea to live in the wasteland",
				"didn't saw their neighbor the Heart of the Wasteland",
				"mutated into superman, but forgot they were irradiated",
				"didn't saw their health, so they they died of radiation poisoning"
			};
			return ROI.ROIMod.instance.rng.Next(deathReason);
		}
	}
}
