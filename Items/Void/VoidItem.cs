using System.Collections.Generic;
using ROI.Players;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Void
{
    //TODO: do all the void item stubs
    internal abstract class VoidItem : ModItem
    {
        protected abstract int Affinity { get; }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "VoidAffinity", $"{Affinity} void affinity")
            {
                overrideColor = new Microsoft.Xna.Framework.Color(153, 102, 255)
            });
        }

        public override bool ConsumeItem(Player player)
        {
            var plr = Main.LocalPlayer.GetModPlayer<ROIPlayer>();
            if (plr.voidAffinity < Affinity) return false;
            plr.voidAffinity -= Affinity;
            return true;
        }
    }
}