using ROI.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Void
{
    //essentially archived for now, until we figure out a real use for it
    internal abstract class VoidItem : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "VoidAffinity", $"{Affinity} void affinity")
            {
                overrideColor = new Microsoft.Xna.Framework.Color(153, 102, 255)
            });
        }

        public override bool CanUseItem(Player player)
        {
            var plr = player.GetModPlayer<ROIPlayer>();

            plr.AddVoidBuff(BuffType, BuffTime);
            return true;
        }


        protected abstract int Affinity { get; }

        protected virtual string BuffType => Name;
        protected abstract int BuffTime { get; }
    }
}