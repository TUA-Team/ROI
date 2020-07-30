using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Weapons.Wasteland
{
    public class VenomousLance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Venomous Lance");
        }

        public override void SetDefaults()
        {
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 22;
			item.useTime = 22;
			item.shootSpeed = 6f;
			item.knockBack = 5f;
			item.width = 46;
			item.height = 54;
			item.damage = 29;
			item.scale = 1.1f;
			item.UseSound = SoundID.Item1;
			item.shoot = ProjectileID.DarkLance;
			item.rare = ItemRarityID.Orange;
			item.value = 27000;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.melee = true;
		}
    }
}
