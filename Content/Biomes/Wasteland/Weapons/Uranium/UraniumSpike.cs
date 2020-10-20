using Microsoft.Xna.Framework;
using ROI.Content.Items;
using ROI.Content.Projectiles.Special;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.Weapons.Uranium
{
    internal class UraniumSpike : ROIItem
    {
        public UraniumSpike() : base("Uranium Spike", "Launches a ground-spike wave", 32, 64) { }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 20;
            item.useAnimation = 20;
            item.noUseGraphic = true;
            item.noMelee = true;
        }

        public override bool UseItem(Player player)
        {
            var pos = player.position.ToTileCoordinates();
            while (!Main.tile[pos.X, pos.Y].nactive())
                pos.Y++;
            pos.Y += 1;
            var proj = Projectile.NewProjectile(pos.ToWorldCoordinates(), Vector2.Zero,
                ModContent.ProjectileType<UraniumSpikeProjectile>(), 0, 0);
            Main.projectile[proj].direction = player.direction;
            return true;
        }

        public override void AddRecipes() => new RecipeBuilder(this)
            //.AddCondition((Mod as ROIMod).DevRecipeCondition)
            .AddIngredient(ItemID.Wood)
            .Register();
    }
}
