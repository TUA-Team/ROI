using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 4;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.scale = 0.5f;
            projectile.damage = 100;
            projectile.ignoreWater = true;
            if (!Main.gameMenu)
            {
                projectile.timeLeft = Main.rand.Next(200, 300);
            }
        }

        public override void AI()
        {
            if (projectile.timeLeft < 25)
            {
                if (projectile.ai[0] == 0)
                {
                    Dust.NewDust(projectile.position, 4, 4, DustID.Stone, 0.1f, 0.3f, 0, new Color(0, 255, 0, 255));
                    Dust.NewDust(projectile.position, 4, 4, DustID.Stone, -0.1f, 0.3f, 0, new Color(0, 255, 0, 255));
                    Dust.NewDust(projectile.position, 4, 4, DustID.Stone, 0f, 0.3f, 0, new Color(0, 255, 0, 255));
                }
                projectile.ai[0] = 1;
                projectile.velocity = Vector2.Zero;
                projectile.Opacity -= 0.05f;
                projectile.frame = 1;
            }
            else
            {
                if (projectile.scale < 1f)
                {
                    projectile.scale += 0.005f;
                }
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
