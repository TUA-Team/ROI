using Microsoft.Xna.Framework;
using ROI.NPCs.Void.VoidPillar;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        private const float _cameraFocusSpeed = 1f;

        public override void ModifyScreenPosition()
        {
            NPC npc = Main.npc.FirstOrDefault(i => i.modNPC is ICamerable camera && camera.CurrentlyExecuting);
            if (npc != null)
            {
                ModNPC modNPC = npc.modNPC;
                if (modNPC is VoidPillar pillar && pillar.CurrentlyExecuting)
                {
                    //CentralizeOnMob(pillar);
                }
            }
        }


        public bool CentralizeOnMob<T>(T mob) where T : ModNPC, ICamerable
        {
            ModNPC mobToCentralizeOn = Main.npc.First(i => i.modNPC != null && i.modNPC.Name == mob.Name).modNPC;

            if (mobToCentralizeOn == null || !mob.ScrollingEffect)
            {
                return false;
            }

            Vector2 cameraPosition = Main.screenPosition;
            Vector2 cameraTarget = mob.GetCameraPosition();
            cameraPosition = Vector2.Lerp(cameraPosition, cameraTarget, 0.6f);
            if ((cameraPosition - cameraTarget).Length() < _cameraFocusSpeed)
            {
                cameraPosition = cameraTarget;
            }

            Main.screenPosition = cameraPosition;
            return true;
        }
    }
}
