using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Weapons.Wasteland
{
    public class WastelandBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wasteland Bomb");
        }

        public override void SetDefaults()
        {
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shootSpeed = 12f;
			item.shoot = ModContent.ProjectileType<Projectiles.WastelandBomb>();
			item.width = 36;
			item.height = 36;
			item.maxStack = 30;
			item.consumable = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 40;
			item.useTime = 40;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
		}

        public override void AddRecipes()
        {
            // TODO:
        }
    }
}
