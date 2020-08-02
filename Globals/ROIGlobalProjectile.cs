using Terraria;
using Terraria.ModLoader;
using static ROI.Globals.ProjectileAIStyle.ProjectileAIStyle;

namespace ROI.Globals
{

    partial class ROIGlobalProjectile : GlobalProjectile
    {
        public static class ProjectileAIStyleID
        {
            public const int LaserBeamAI = 84;
        }

        public override bool PreAI(Projectile projectile)
        {
            if (projectile.aiStyle == ProjectileAIStyleID.LaserBeamAI)
            {
                DeathRayAIStyle(projectile);
                return false;
            }
            return base.PreAI(projectile);
        }
    }
}
