using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Projectiles
{
    internal class TeslaOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.height = 32;
            projectile.width = projectile.height;
            projectile.timeLeft = 2;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
        }

        private int Radius { get => (int)projectile.ai[0]; set => projectile.ai[0] = value; }
        private float Rotation { get => projectile.ai[1]; set => projectile.ai[1] = value; }
        private float CurrentRadius { get; set; }

        //todo: make rotation perfect circle
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!player.active || player.dead)
            {
                projectile.Kill();
                return;
            }
            if (Radius == 0)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Vector2 offset = Main.MouseWorld - player.position;
                    //Radius = (int)System.Math.Ceiling(offset.Length() / 6f) * 6;
                    Radius = (int)offset.Length();
                    Rotation = offset.ToRotation();
                    projectile.netUpdate = true;
                    return;
                }
                CurrentRadius = 0;
            }
            Rotation += 0.02f;
            Rotation %= MathHelper.TwoPi;
            projectile.Center = player.Center + CurrentRadius * Rotation.ToRotationVector2();
            if (CurrentRadius < Radius)
            {
                CurrentRadius += 6;
            }
            projectile.rotation -= 0.05f;
            projectile.localAI[0] += 0.2f;
            projectile.localAI[0] %= 4f;
            projectile.timeLeft = 2;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int k = projectile.whoAmI + 1; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.active && proj.owner == projectile.owner && proj.type == projectile.type)
                {
                    if (BeamCheck(proj, targetHitbox)) return true;
                }
            }
            return null;
        }

        public override bool? CanCutTiles() => false;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) =>
            target.immune[projectile.owner] = 4;

        private bool BeamCheck(Projectile proj, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, proj.Center, 6f, ref point);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int k = projectile.whoAmI + 1; k < 1000; k++)
            {
                Projectile destinationProj = Main.projectile[k];
                if (destinationProj.active && destinationProj.owner == projectile.owner && destinationProj.type == projectile.type)
                {
                    Texture2D texture = mod.GetTexture("Projectiles/TeslaOrbBeam");
                    Vector2 unit = destinationProj.Center - projectile.Center;
                    float length = unit.Length();
                    unit.Normalize();
                    float rotation = unit.ToRotation();
                    for (float j = projectile.localAI[0]; j <= length; j += 8f)
                    {
                        Vector2 drawPos = projectile.Center + unit * j - Main.screenPosition;
                        if (drawPos.X < 0 || drawPos.Y < 0) continue;
                        spriteBatch.Draw(texture, drawPos, null,
                            Color.White,
                            rotation, new Vector2(4, 4), 1f,
                            SpriteEffects.None, 0f);
                    }
                }
            }
            return true;
        }
    }
}