using Microsoft.Xna.Framework;
using ROI.Models.Enums;
using Terraria;

namespace ROI.NPCs.Bosses.VoidPillar
{
    public sealed partial class VoidPillar
    {
        // TODO: (low prio) move to SetDefaults()
        private int _shockwaveTimer = 300;

        internal void Shockwave()
        {
            _shockwaveTimer--;
            if (_shockwaveTimer != 0)
            {
                return;
            }

            _shockwaveTimer = 500;

            switch (ShieldColor)
            {
                case PillarShieldColor.Red:
                    Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, mod.ProjectileType("VoidPillarTeleportationShockwave"), 10, 0.5f, Main.myPlayer, 1f, 0f);
                    break;
            }
        }
    }
}
