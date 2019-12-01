using ROI.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Lunar
{
    internal abstract class LunarAnchor : ModItem
    {
        protected abstract int AnchorValue { get; }

        public sealed override void UpdateInventory(Player player)
		{
            var plr = player.GetModPlayer<ROIPlayer>();
			SafeUpdateInventory(player, plr);
            plr.lunarAmassMax = System.Math.Max(plr.lunarAmassMax, AnchorValue);
        }
		
		protected virtual void SafeUpdateInventory(Player player, ROIPlayer plr)
		{
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "LunarAnchorValue", $"{AnchorValue} void affinity")
            {
                overrideColor = new Microsoft.Xna.Framework.Color(153, 102, 255)
            });
        }
    }
}
