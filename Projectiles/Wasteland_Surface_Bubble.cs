using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiquidAPI.LiquidMod;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Projectiles
{
    /// <summary>
    /// ai[0] = popped out
    /// </summary>
    class Wasteland_Surface_Bubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radioactive Bubble");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.scale = 0.5f;
            projectile.damage = 100;
            projectile.ignoreWater = true;
            if (!Main.gameMenu)
            {
                projectile.timeLeft = 200;
                projectile.frame = 0;
            }
        }

        public override void AI()
        {
            if (projectile.timeLeft < 175)
            {
                projectile.frame = 1;
            }
            if (projectile.timeLeft < 150)
            {
                projectile.frame = 2;
            }
            if (projectile.timeLeft < 125)
            {
                projectile.frame = 3;
            }
            if (projectile.timeLeft < 100)
            {
                projectile.frame = 4;
            }
            if (projectile.timeLeft < 25)
            {
                if (projectile.ai[0] == 0)
                {
                    Dust.NewDust(projectile.position, 4, 4, DustID.Stone, 0.1f, 0.3f, 0, new Color(0, 255, 0, 255));
                    Dust.NewDust(projectile.position, 4, 4, DustID.Stone, -0.1f, 0.3f, 0, new Color(0, 255, 0, 255));
                    Dust.NewDust(projectile.position, 4, 4, DustID.Stone, 0f, 0.3f, 0, new Color(0, 255, 0, 255));
                }
                projectile.ai[0] = 1;
                projectile.Opacity -= 0.05f;
                projectile.frame = 5;
            }
            else
            {
            }

            Vector2 projectileInWorldPosition = projectile.BottomLeft / 16;
            LiquidRef liquid = LiquidWorld.grid[(int) projectileInWorldPosition.X, (int) (projectileInWorldPosition.Y)];
            if (liquid.Amount < 200)
            {
                //projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(projectile.ai[0] == 0)
            {
                projectile.timeLeft = 25;
            }
            return base.OnTileCollide(oldVelocity);
        }
    }
}
