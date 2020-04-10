using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Irrawood
{
    abstract class IrrawoodArmor : ArmorBase
    {
        public sealed override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<IrrawoodHelmet>()
                && body.type == ModContent.ItemType<IrrawoodChestplate>()
                && legs.type == ModContent.ItemType<IrrawoodLeggings>();
        }
    }
}
