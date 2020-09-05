using Microsoft.Xna.Framework;
using ROI.NPCs.Void.VoidPillar;
using ROI.Projectiles;
using ROI.Projectiles.Beams;
using System;
using System.Collections.Generic;
using System.Linq;
using ROI.Worlds.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Weapons.Solar
{
    class SolarWind : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar wind");
            Tooltip.SetDefault("May the solar wind be with you!\n" +
                               "This item cannot be reforged");
        }

        public override void SetDefaults()
        {
            item.magic = true;
            item.mana = 0;
            item.damage = 150;
            item.damage = 20;
            item.width = 40;
            item.height = 20;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = 10000;
            item.rare = 2;
            item.autoReuse = false;
            item.shootSpeed = 16f;
            item.shoot = mod.ProjectileType("Brimstone");
            base.SetDefaults();
        }

        public override bool UseItem(Player player)
        {
            Subworld.Enter<VoidRiftSubworld>(true);
            return false;
            List<NPC> pillars = Main.npc.Where(i => i.modNPC is VoidPillar).ToList();
            foreach (NPC pillar in pillars)
            {
                pillar.ai[0] = 1f;
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Subworld.Enter<VoidRiftSubworld>();
            return false;
            int count = 64;

            //set start angle to base angle
            float currentAngle = 0;
            float perCurve = MathHelper.TwoPi / count;

            //parameters for you to change
            float length = 160f;
            float tangentLength = 100f;

            for (int i = 0; i < count; i++)
            {
                Vector2 perp = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                Vector2 tang = new Vector2(perp.Y, -perp.X);

                Brimstone brimstone = Projectile.NewProjectileDirect(position, Vector2.Zero, mod.ProjectileType("Brimstone"), 0, 0, Main.myPlayer, 0f, currentAngle).modProjectile as Brimstone;

                brimstone.curve = new BaseCurvingBeam.BezierCurve(
                    position,
                    position + perp * length * 0.75f,
                    position + perp * length + tang * tangentLength * 0.5f,
                    position + perp * length + tang * tangentLength);

                brimstone.projectile.ai[1] = currentAngle;
                brimstone.projectile.netUpdate = true;

                Main.projectile[brimstone.projectile.whoAmI] = brimstone.projectile;

                currentAngle += perCurve;
            }
            return false;
        }


        public override bool NewPreReforge()
        {
            return false;
        }
    }
}
