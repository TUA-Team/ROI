using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Weapons.Wasteland
{
    class VenomousGreatBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Greatsword");
            Tooltip.SetDefault("Strike your foes with a deadly toxic lash");
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.damage = 38;
            item.knockBack = 0.2f;
            item.melee = true;
            item.consumable = false;
            item.width = 56;
            item.height = 56;
            item.value = Item.sellPrice(0, 0, 55, 0);
            item.useTime = 30;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.autoReuse = false;
            
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(100) > 45)
            {
                target.AddBuff(BuffID.Venom, 180, true);
            }

            target.life = 1;
            target.StrikeNPCNoInteraction(1, 0, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("Wasteland_Ingot"), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            // Link to TUA.AddRecipes
        }
    }
}
