using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Summons
{
    class ClumpOfRadioactiveMeat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clump of radioactive meat");
            Tooltip.SetDefault("The nurse said it was not alive, but you can ear it beating");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 46;
            item.maxStack = 1;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 0;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.position.Y / 16 > Main.maxTilesY - 200)
            {
                Projectile.NewProjectile(player.position, (player.Center - Main.MouseWorld) / 10, ModContent.ProjectileType<Projectiles.HeartOfTheWasteland.ClumpOfRadioactiveMeat>(), 0 , 0);
                return true;
            }
            return false;
        }
    }
}
