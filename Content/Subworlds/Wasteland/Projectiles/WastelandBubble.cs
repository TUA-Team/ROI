using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Projectiles
{
    /// <summary>
    /// ai[0] = bubble size
    /// ai[1] = state
    /// localai[0] = property setted
    /// </summary>
    internal class WastelandBubble : ModProjectile
    {

        private static readonly List<Rectangle> smallBubble = new List<Rectangle>()
        {
            new Rectangle(0, 0, 16, 16), //First frame
            new Rectangle(0, 18, 16, 16) //Second frame
        };

        private static readonly List<Rectangle> mediumBubble = new List<Rectangle>()
        {
            new Rectangle(18, 0, 32, 32), //First frame
            new Rectangle(18, 32, 32, 32) //Second frame
        };

        private static readonly List<Rectangle> bigBubble = new List<Rectangle>()
        {
            new Rectangle(52, 0, 48, 48), //First frame
            new Rectangle(52, 48, 48,48) //Second frame
        };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wasteland Radioactive Bubble");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            if (!Main.gameMenu)
            {
                projectile.timeLeft = 500;
                projectile.frame = 0;
            }
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                switch (projectile.ai[0])
                {
                    case 0:
                        projectile.width = 16;
                        projectile.height = 16;
                        projectile.damage = 25;
                        break;
                    case 1:
                        projectile.width = 32;
                        projectile.height = 32;
                        projectile.damage = 50;
                        break;
                    case 2:
                        projectile.width = 48;
                        projectile.height = 48;
                        projectile.damage = 100;
                        break;
                }
            }

            if (projectile.timeLeft < 25)
            {
                projectile.ai[1] = 1;
            }


            if ((int)projectile.ai[1] == 1)
            {
                projectile.velocity = Vector2.Zero;
                projectile.Opacity -= 0.05f;
            }
            //Make the bubble pop out if another projectile hit this projectile
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                var proj = Main.projectile[i];

                // TODO: (low prio) this is probably not very performant, but it should be fine for good pcs
                if (proj.active && proj.Hitbox.Intersects(projectile.Hitbox) &&
                    proj.type != projectile.type && proj.type != ModContent.ProjectileType<WastelandSurfaceBubble>())
                {
                    projectile.timeLeft = 26;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //oldVelocity = Vector2.Zero;
            projectile.timeLeft = 26;
            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.timeLeft = 26;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.timeLeft = 26;
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle bubbleSpriteRectangle = new Rectangle();

            switch (projectile.ai[0])
            {
                case 0:
                    bubbleSpriteRectangle = smallBubble[(int)projectile.ai[1]];
                    break;
                case 1:
                    bubbleSpriteRectangle = mediumBubble[(int)projectile.ai[1]];
                    break;
                case 2:
                    bubbleSpriteRectangle = bigBubble[(int)projectile.ai[1]];
                    break;
                default:
                    projectile.Kill();
                    break;
            }
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition, bubbleSpriteRectangle, Color.White * projectile.Opacity);
            return false;
        }
    }
}