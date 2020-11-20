using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland
{
    internal sealed class WastelandPlayer : ModPlayer
    {
        bool wastelandFilter;
        readonly Color color = new Color(64, 0, 0);
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
