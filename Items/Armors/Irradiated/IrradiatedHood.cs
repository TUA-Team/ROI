using ROI.Players;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Irradiated
{
    [AutoloadEquip(EquipType.Head)]
    class IrradiatedHood : ArmorBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irradiated Hood");
            Tooltip.SetDefault("+2% magic damage\n" +
                               "+2% magic crit");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("IrradiatedChestplate") && legs.type == mod.ItemType("IrradiatedLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+20 mana\n" +
                              "+A radiation aura is surrounding you burning any enemy that is inside of it\n" +
                              "Gain radiation slower in the wasteland\n" +
                              "Liquid waste have even more reduced effect\n" +
                              "Balanced?";
            player.statManaMax2 += 20;
            player.GetModPlayer<ROIPlayer>().irradiatedSet = true;
            player.GetModPlayer<ROIPlayer>().irradiatedHood = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage *= 0.02f;
            player.magicCrit += 2;
        }
    }
}
