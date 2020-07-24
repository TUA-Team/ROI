using ROI.Players;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Irradiated
{
    [AutoloadEquip(EquipType.Head)]
    class IrradiatedMask : ArmorBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irradiated Mask");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.defense = 7;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("IrradiatedChestplate") && legs.type == mod.ItemType("IrradiatedLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+25 maximum health max\n" +
                              "Gain radiation slower in the wasteland\n" +
                              "Liquid waste have even more reduced effect\n" +
                              "Balanced?";
            player.statLifeMax2 += 25;
            player.GetModPlayer<ROIPlayer>().irradiatedSet = true;
            player.GetModPlayer<ROIPlayer>().irradiatedMask = true;
        }
    }
}
