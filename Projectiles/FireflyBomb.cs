using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Projectiles
{
    // A lot of this was taken from Example Mod, so I'm gonna put it off for now
    public class FireflyBomb : ModProjectile
    {
        public override bool Autoload(ref string name) => false;

        public override void SetDefaults()
        {
            // sprite is 20x26
            projectile.width = 18;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
            projectile.tileCollide = true;

            drawOffsetX = 10;
            drawOriginOffsetY = 13;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.expertMode && target.type >= NPCID.EaterofWorldsHead
                && target.type <= NPCID.EaterofWorldsTail)
            {
                damage = (int)(damage * .2f);
            }
        }

        public override void AI()
        {
            projectile.rotation++;
        }

        public override void Kill(int timeLeft)
        {
            // Play glass sound
            Main.PlaySound(SoundID.Shatter, projectile.position);
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 
                    projectile.width, projectile.height, DustID.Fire, Scale: 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 
                    projectile.width, projectile.height, DustID.Fire, Scale: 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            int goreIndex;
            // Large Smoke Gore spawn
            for (int g = 0; g < 8; g++)
            {
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + projectile.width * .5f - 24f, 
                        projectile.position.Y + projectile.height * .5f - 24f),
                    default, Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
            }
            // reset size to normal width and height.
            projectile.position.X = projectile.position.X + projectile.width * .5f;
            projectile.position.Y = projectile.position.Y + projectile.height * .5f;
            projectile.width = 18;
            projectile.height = 24;
            projectile.position.X = projectile.position.X - projectile.width * .5f;
            projectile.position.Y = projectile.position.Y - projectile.height * .5f;

            // damage enemies
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].DistanceSQ(projectile.position) < 50)
                {
                    Main.npc[i].life -= 15;
                    Main.npc[i].GetGlobalNPC<NPCs.Globals.EffectNPC>().fireflyStunned = 45;
                    NetMessage.SendData(MessageID.StrikeNPC, -1, Main.myPlayer, null, i, 15, 2f,
                        Main.npc[i].position.X.CompareTo(projectile.position.X));
                    var p = mod.GetPacket();
                    p.Write((byte)ID.NetworkMessage.FireflyStun);
                    p.Write((byte)i);
                    p.Send();
                }
            }
        }
    }
}
