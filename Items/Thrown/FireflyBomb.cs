using Terraria;
using Terraria.ID;

namespace ROI.Items.Thrown
{
    public class FireflyBomb : ROIItem
    {
        public override bool Autoload(ref string name) => false;

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.shootSpeed = 12f;
            item.shoot = mod.ProjectileType<Projectiles.FireflyBomb>();
            item.width = 18;
            item.height = 24;
            item.maxStack = 30;
            item.consumable = true;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 40;
            item.useTime = 40;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.value = Item.buyPrice(0, 0, 20, 0);
            item.rare = 1;
        }
    }
}
