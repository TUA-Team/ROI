using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Globals
{
    internal partial class ROIGlobalItem : GlobalItem
    {


        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.DemonHeart)
            {
                item.consumable = false;
                item.accessory = true;

            }
        }

        public override bool UseItem(Item item, Player player)
        {
            if (item.type == ItemID.DemonHeart)
            {
                return false;
            }
            return base.UseItem(item, player);
        }


        public override void PostUpdate(Item item)
        {

            base.PostUpdate(item);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.DemonHeart)
            {
                int index = tooltips.FindIndex(i => i.mod == "Terraria" && i.Name == "Tooltip0");
                if (index != -1)
                {
                    tooltips[index] = new TooltipLine(mod, "Tooltip1", "5% damage reduction in hell\nKilling demon grant you the demon fury buff");
                }
            }

            base.ModifyTooltips(item, tooltips);
        }
    }
}
