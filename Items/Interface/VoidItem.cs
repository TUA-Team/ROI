using System.Collections.Generic;
using Terraria.ModLoader;

namespace ROI.Items.Interface
{
    public abstract class VoidItem : ModItem
    {
        public abstract int VoidTier { get; set; }
        public abstract int VoidCost { get; set; }

        /// <summary>
        /// To use with void tier
        /// </summary>
        public abstract void VoidTierEffect();

        public sealed override void SetDefaults()
        {
            TrueSetDefaults();
            item.summon = false;
            item.magic = false;
            item.thrown = false;
            item.melee = false;
            item.ranged = false;
        }

        protected abstract void TrueSetDefaults();

        // void VoidPossession();

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(i => i.mod == "Terraria" && i.Name == "Expert");
            if (index == -1) index = tooltips.Count;
            TooltipLine voidCostLine = new TooltipLine(mod, "VoidSpecification", $"Cost: {VoidCost} [c/800080:void affinity points]");
            tooltips.Insert(index - 1, voidCostLine);
            TrueModifyTooltips(tooltips, index);
        }

        protected virtual void TrueModifyTooltips(List<TooltipLine> tooltips, int prevLnIndex) { }
    }
}
