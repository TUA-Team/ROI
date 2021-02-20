using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland
{
    internal sealed class WastelandPlayer : ModPlayer
    {
        public bool grasped;

        public override void ResetEffects()
        {
            grasped = false;
        }

        public override void PreUpdateBuffs()
        {
            if (WastelandWorld.activeHotW != -1)
            {
                if (player.position.Y / 16 > Main.maxTilesY - 250)
                {
                    player.AddBuff(BuffID.Horrified, 2);
                }

                if (Main.npc[WastelandWorld.activeHotW].ai[0] == 1 &&
                    player.position.X / 16 > Main.npc[WastelandWorld.activeHotW].position.X / 16 - 300 ||
                    player.position.X / 16 < Main.npc[WastelandWorld.activeHotW].position.X / 16 + 300)
                {
                    grasped = true;
                    player.AddBuff(ModContent.BuffType<HeartOfTheWasteland.Debuff.Grasped>(), 2);
                }
            }
        }

        /*bool wastelandFilter;
        readonly Color color = new Color(64, 0, 0);*/
/*        public override void UpdateBiomeVisuals()
        {
            if (Subworld.IsActive<WastelandDepthSubworld>())
            {
                float percent = (player.position.Y / 16 - Main.maxTilesY + 300) / 300;
                if (!wastelandFilter)
                {
                    Filters.Scene.Activate("ROI:UnderworldFilter", player.Center)
                        .GetShader().UseColor(color).UseIntensity(percent).UseOpacity(percent);
                    wastelandFilter = true;
                }
                Filters.Scene["ROI:UnderworldFilter"].GetShader().UseColor(0f, 0f, 1f).UseIntensity(0.5f).UseOpacity(1f);
            }
            else
            {
                if (wastelandFilter)
                {
                    Filters.Scene["ROI:UnderworldFilter"].Deactivate();
                    wastelandFilter = false;
                }
            }
        }*/
    }
}
