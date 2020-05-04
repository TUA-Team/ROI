using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Projectiles
{
	/// <summary>
	/// Projectile.ai[0] = is MainWave 0 = false, 1 = true
	/// Projectile.ai[1] = Wave number
	/// </summary>
	abstract class BaseShockwave : ModProjectile
	{
		public sealed override string Texture => "ROI/Projectiles/BlankProjectile";

		internal virtual Color ShockwaveColor => Color.White;

		internal virtual float ProjectileDestructionChance => 1f;

		public override bool CloneNewInstances => false;

		protected virtual int RippleCount { get; } = 3;
		protected virtual int RippleSize { get; } = 5;
		protected virtual int RippleSpeed { get; } = 15;
		protected virtual float DistortStrength { get; } = 100f;

		protected virtual int WaveAmount { get; set; } = 2;

		protected virtual int MaxTimerBetweenWave { get; } = 180;
		private int _timer;

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = true;
			projectile.magic = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.timeLeft = 300;
			projectile.penetrate = 900;
			projectile.tileCollide = false;
			_timer = MaxTimerBetweenWave;
		}

		public override void AI()
		{
			foreach (Projectile proj in Main.projectile.Where(i => i.Hitbox.Intersects(projectile.Hitbox)))
			{
				if (!(proj.modProjectile is BaseShockwave) && !proj.minion && this.Colliding(projectile.Hitbox, proj.Hitbox).Value)
				{
					Dust.NewDust(proj.position, proj.width, proj.height, DustID.Fire, -proj.velocity.X / 100, -proj.velocity.Y / 100, 255, ShockwaveColor, 2f);
					proj.active = false;
					proj.netUpdate = true;
				}
			}

			projectile.velocity = Vector2.Zero;
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width += 8;
			projectile.height += 8;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			if (Main.netMode != 2)
			{
				if (!Filters.Scene[$"ROI:Shockwave{projectile.ai[1]}"].IsActive())
				{
					Filters.Scene.Activate($"ROI:Shockwave{projectile.ai[1]}", projectile.Center).GetShader().UseColor(RippleCount, RippleSize, RippleSpeed).UseTargetPosition(projectile.Center).UseSecondaryColor(2f, 2f, 2f);
				}

				float progress = (300f - projectile.timeLeft) / 60f;
				Filters.Scene[$"ROI:Shockwave{projectile.ai[1]}"].GetShader().UseProgress(progress).UseOpacity(DistortStrength * (1 - progress / 3f)).UseColor(2, 5, 6).UseTargetPosition(projectile.Center);
			}

			if (WaveAmount == 0)
			{
				projectile.ai[1] = 9f; //Use the last shockwave slot instead
			}

			_timer--;
			if (_timer == 0 && projectile.ai[0] == 1f && WaveAmount != 0)
			{
				Projectile.NewProjectileDirect(projectile.Center, projectile.velocity, mod.ProjectileType(this.GetType().Name), projectile.damage, projectile.knockBack, projectile.owner, WaveAmount, 0).ai[0] = WaveAmount;
				WaveAmount--;
				_timer = MaxTimerBetweenWave;
				projectile.netUpdate = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			Filters.Scene[$"ROI:Shockwave{projectile.ai[1]}"].Deactivate();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Rectangle up = UpperCollision(projHitbox);
			Rectangle down = BottomCollision(projHitbox);
			Rectangle left = LeftCollision(projHitbox);
			Rectangle right = RightCollision(projHitbox);

			return IsTargetColliding(up, targetHitbox) || IsTargetColliding(down, targetHitbox) || IsTargetColliding(left, targetHitbox) || IsTargetColliding(right, targetHitbox);
		}

		private Rectangle UpperCollision(Rectangle originalHitbox)
		{
			return new Rectangle(originalHitbox.X, originalHitbox.Y, originalHitbox.Width, 10);
		}

		private Rectangle BottomCollision(Rectangle originalHitbox)
		{
			return new Rectangle(originalHitbox.X, originalHitbox.Y + originalHitbox.Height, originalHitbox.Width, 10);
		}

		private Rectangle LeftCollision(Rectangle originalHitbox)
		{
			return new Rectangle(originalHitbox.X, originalHitbox.Y, 10, originalHitbox.Height);
		}

		private Rectangle RightCollision(Rectangle originalHitbox)
		{
			return new Rectangle(originalHitbox.X + originalHitbox.Width, originalHitbox.Y, 10, originalHitbox.Height);
		}

		private bool IsTargetColliding(Rectangle projHibox, Rectangle targetHitbox)
		{
			return targetHitbox.Intersects(projHibox);
		}

		public override bool CanHitPlayer(Player target)
		{
			return true;
		}
	}
}
