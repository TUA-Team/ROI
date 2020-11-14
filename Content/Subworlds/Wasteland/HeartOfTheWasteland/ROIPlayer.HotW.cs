using ROI.Content.Subworlds.Wasteland.HeartOfTheWasteland.Debuff;
using ROI.Content.Worlds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Players
{
    partial class ROIPlayer
    {
        public bool grasped;


        private void ResetEffectsWasteland()
        {
            grasped = false;
        }


        private void PostUpdateWasteland()
        {
            if (ROIWorld.activeHotWID != -1)
            {
                if (player.position.Y / 16 > Main.maxTilesY - 250)
                {
                    player.AddBuff(BuffID.Horrified, 2);
                }

                if (Main.npc[ROIWorld.activeHotWID].ai[0] == 1 &&
                    player.position.X / 16 > Main.npc[ROIWorld.activeHotWID].position.X / 16 - 300 ||
                    player.position.X / 16 < Main.npc[ROIWorld.activeHotWID].position.X / 16 + 300)
                {
                    grasped = true;
                    player.AddBuff(ModContent.BuffType<Grasped>(), 2);
                }
            }
        }
    }
}
