using Terraria.ModLoader;
using Terraria.ID;
using ROI.Items.Placeables.Wasteland;

namespace ROI.Items.Weapons.Wasteland
{
    public class DesolateBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desolate Bow");
        }

        public override void SetDefaults()
        {
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 22;
			item.useTime = 22;
			item.width = 14;
			item.height = 32;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 31;
			item.shootSpeed = 8f;
			item.knockBack = 2f;
			item.alpha = 30;
			item.rare = ItemRarityID.Orange;
			item.noMelee = true;
			//item.scale = 1.1f;
			item.value = 27000;
			item.ranged = true;
		}

        public override void AddRecipes()
        {
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wasteland_Ingot>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
