using Microsoft.Xna.Framework;
using Terraria;

namespace ROI.Projectiles
{
    class DecorativeShockwave : BaseShockwave
    {
        internal override Color ShockwaveColor => Color.White;

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.damage = 0;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
