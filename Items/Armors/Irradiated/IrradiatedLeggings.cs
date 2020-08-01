using Terraria.ModLoader;

namespace ROI.Items.Armors.Irradiated
{
    [AutoloadEquip(EquipType.Legs)]
    class IrradiatedLeggings : ArmorBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irradiated Leggings");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.defense = 7;
        }
    }
}
