using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.Enums;
using ROI.NPCs.Interfaces;
using Terraria;
using Terraria.ModLoader;

namespace ROI.NPCs.Void.VoidPillar
{
	internal sealed partial class VoidPillar : ModNPC, ISavableEntity, ICamerable, IMobCamerable<VoidPillar>
	{
		private int _shockwaveTimer = 300;

		internal void Shockwave()
		{
			_shockwaveTimer--;
			if (_shockwaveTimer != 0)
			{
				return;
			}

			_shockwaveTimer = 300;

			switch (ShieldColor)
			{
				case PillarShieldColor.Red:
					Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("VoidPillarRedShockwave"), 10, 0.5f);
					break;
			}
		}
	}
}
