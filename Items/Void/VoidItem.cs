using ROI.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Void
{
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
            if (plr.voidAffinity >= Affinity)
            {
                plr.voidAffinity -= Affinity;
                if (--item.stack == 0) item.TurnToAir();
                plr.AddVoidBuff(BuffType, BuffTime);
                return true;
            }
            return false;
        }


        protected abstract int Affinity { get; }

        protected virtual string BuffType => Name;
        protected abstract int BuffTime { get; }
    }
}