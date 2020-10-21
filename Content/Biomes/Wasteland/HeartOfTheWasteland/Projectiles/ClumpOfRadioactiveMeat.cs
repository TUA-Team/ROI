using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.HeartOfTheWasteland.Projectiles
{
    internal class ClumpOfRadioactiveMeat : ModProjectile
    {
        private bool bossSummoned;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radioactive meat");
        }

        public override void SetDefaults()
        {
            projectile.tileCollide = true;
            projectile.width = 32;
            projectile.height = 46;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 0;
            projectile.aiStyle = -1;
            projectile.timeLeft = 600;
        }

        /// <summary>
        /// Logic:
        /// while the velocity of the projectile is not 0, it cannot attract the heart of the wasteland
        /// which mean that the clump can be taken back. <br />
        /// Once the velocity reach 0, the boss will start to draw and dust will spawn as if the boss was eating. <br />
        /// Once the boss is done eating, it will roar and release a powerful shockwave to start the fight (this doesn't do any damage)
        /// </summary>
        public override void AI()
        {
            if (projectile.position.Y / 16 < Main.maxTilesY - 200)
            {
                ConvertBackToItem();
                return;
            }

            if (bossSummoned)
            {
                return;
            }

            //Start summoning
            if (projectile.velocity.X < 0.2f && projectile.velocity.X < 0.2f && !NPC.AnyNPCs(mod.NPCType("HeartOfTheWasteland")))
            {
                ModNPC bossInfo = mod.GetNPC("HeartOfTheWasteland");
                NPC.NewNPC((int)(projectile.position.X + bossInfo.npc.width / 2 - 55), (int)(projectile.position.Y + bossInfo.npc.height - 28), bossInfo.npc.type, 0, 0, 0, projectile.whoAmI);
            }
            else
            {
                projectile.velocity /= 1.1f;
            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ConvertBackToItem();
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (projectile.velocity.X > 0.2f && projectile.velocity.Y > 0.2f)
                ConvertBackToItem();
        }

        public override bool PreKill(int timeLeft)
        {
            if (NPC.AnyNPCs(NPCID.Nurse))
            {
                NPC npc = Main.npc.FirstOrDefault(i => i.type == NPCID.Nurse);
                npc.StrikeNPC(100000, 1f, 1, true, false, false);
            }
            return base.PreKill(timeLeft);
        }

        public void ConvertBackToItem()
        {
            projectile.Kill();
            Item.NewItem(projectile.position, ModContent.ItemType<Summon.ClumpOfRadioactiveMeat>(), 1);
        }
    }
}
