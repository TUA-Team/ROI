using Microsoft.Xna.Framework;
using ROI.NPCs.Void.VoidPillar;
using Terraria;
using Terraria.DataStructures;

namespace ROI.Projectiles.VoidPillarProjectiles
{
	class VoidPillarTeleportationShockwave : BaseShockwave
	{
		internal override Color ShockwaveColor => Color.Transparent;

		internal override float ProjectileDestructionChance => 1f;

		protected override int WaveAmount { get; set; } = 5;
		protected override int RippleCount => 5;
		protected override float DistortStrength => 100f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void pillar teleportation shockwave");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.damage = 1;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.Hurt(PlayerDeathReason.ByCustomReason(target.name + " got destroyed by a void fracture shockwave"), 100, 0, false, false, false, 10);
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
