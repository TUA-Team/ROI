using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.NPCs.Void.VoidPillar;
using Terraria;
using Terraria.DataStructures;

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
