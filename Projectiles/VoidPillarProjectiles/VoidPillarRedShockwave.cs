using Microsoft.Xna.Framework;
using ROI.Buffs.Void;
using ROI.NPCs.Void.VoidPillar;
using Terraria;
using Terraria.DataStructures;

namespace ROI.Projectiles.VoidPillarProjectiles
{
	class VoidPillarRedShockwave : BaseShockwave
	{
		internal override Color ShockwaveColor => Color.Red;

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.damage = 10;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.Hurt(PlayerDeathReason.ByCustomReason(target.name + " got struck by temporal shockwave from the void pillar"), 5, 0, false, false, false, 10);
			target.AddBuff(mod.BuffType("VoidSlowness"), 300);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.StrikeNPCNoInteraction(10, 0f, 0, false, false, true);
		}

		public override bool? CanHitNPC(NPC target)
		{
			return !(target.modNPC is VoidPillar);
		}
	}
}
