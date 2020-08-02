using ROI.Items.Placeables.Wasteland;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Weapons.Wasteland
{
    public class CorrosiveTraceRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrosive Trace Rifle");
        }

        public override void SetDefaults()
        {
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 11;
            item.useTime = 11;
            item.width = 24;
            item.height = 22;
            item.shoot = ProjectileID.Bullet;
            item.knockBack = 2f;
            item.useAmmo = AmmoID.Bullet;
            item.UseSound = SoundID.Item41;
            item.damage = 24;
            item.shootSpeed = 13f;
            item.noMelee = true;
            item.value = 50000;
            //item.scale = 0.85f;
            item.rare = ItemRarityID.Orange;
            item.ranged = true;
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Handgun);
            recipe.AddIngredient(ModContent.ItemType<Wasteland_Ingot>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
