using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Weapons.Dev
{
    internal class TeslaLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tesla Launcher");
            Tooltip.SetDefault("Developer item"
                + "\n<right> to despawn orbs");
        }

        public override void SetDefaults()
        {
            item.damage = 100;
            item.magic = true;
            item.width = 48;
            item.height = 48;
            item.useTime = 31;
            item.useAnimation = 31;
            item.UseSound = SoundID.Item44;
            item.noMelee = true;
            item.useStyle = 1;
            item.knockBack = 3.5f;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.rare = 11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TeslaOrb");
            item.mana = 25;
        }

        public override bool AltFunctionUse(Player player)
        {
            for (int k = 0; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.type == mod.ProjectileType("TeslaOrb") && proj.active
                    && proj.owner == player.whoAmI)
                {
                    proj.Kill();
                }
            }
            return false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int count = 0;
            for (int k = 0; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.type == mod.ProjectileType("TeslaOrb") && proj.active
                    && proj.owner == player.whoAmI)
                {
                    if (count++ == 9)
                    {
                        AltFunctionUse(player);
                    }
                }
            }
            return true;
        }
    }
}