using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Enemies
{
    // Reference: Voodoo Demon
    public sealed class MutatedDemon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 2f;
            npc.aiStyle = -1;

            npc.width = 82;
            npc.height = 68;

            npc.lifeMax = 140;
            npc.damage = 32;
            npc.defense = 8;
            npc.knockBackResist = 0.8f;

            npc.HitSound = SoundID.NPCHit21;
            npc.DeathSound = SoundID.NPCDeath24;
            npc.value = 1000f;

            npc.lavaImmune = true;
            npc.noGravity = true;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                SetProjMax();
            }
        }


        private int RetreatTimer
        {
            get => (int)npc.ai[0];
            set => npc.ai[0] = value;
        }
        private int RetreatDirectionalTimer
        {
            get => (int)npc.ai[1];
            set => npc.ai[1] = value;
        }
        private Player PlayerTarget => Main.player[npc.target];
        private bool CanHitTarget => Collision.CanHit(npc.position,
                npc.width,
                npc.height,
                PlayerTarget.position,
                PlayerTarget.width,
                PlayerTarget.height);

        private int attackTimer;
        private int projCount;
        private int projMax;

        public override void AI()
        {
            HandleTileCollision();

            if (++RetreatTimer > 200)
                RetreatMovement();
            else
            {
                npc.TargetClosest();

                BaseMovement();
            }

            // We don't want these to run on clients since they use randomized behaviors
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (++attackTimer % 20 == 0 && projCount != projMax)
            {
                projCount++;
                FireProjectile();
            }
            else if (attackTimer >= 300 + Main.rand.Next(300))
            {
                attackTimer = projCount = 0;
                SetProjMax();
            }
        }

        private void HandleTileCollision()
        {
            if (npc.collideX)
            {
                npc.velocity.X = npc.oldVelocity.X * -0.5f;
                if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                    npc.velocity.X = 2f;

                if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                    npc.velocity.X = -2f;
            }

            if (npc.collideY)
            {
                npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
                if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                    npc.velocity.Y = 1f;

                if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                    npc.velocity.Y = -1f;
            }
        }

        private void BaseMovement()
        {
            if (npc.direction == -1 && npc.velocity.X > -4f)
            {
                npc.velocity.X -= 0.1f;
                if (npc.velocity.X > 4f)
                    npc.velocity.X -= 0.1f;
                else if (npc.velocity.X > 0f)
                    npc.velocity.X += 0.05f;

                if (npc.velocity.X < -4f)
                    npc.velocity.X = -4f;
            }
            else if (npc.direction == 1 && npc.velocity.X < 4f)
            {
                npc.velocity.X += 0.1f;
                if (npc.velocity.X < -4f)
                    npc.velocity.X += 0.1f;
                else if (npc.velocity.X < 0f)
                    npc.velocity.X -= 0.05f;

                if (npc.velocity.X > 4f)
                    npc.velocity.X = 4f;
            }

            if (npc.directionY == -1 && npc.velocity.Y > -1.5)
            {
                npc.velocity.Y -= 0.04f;
                if (npc.velocity.Y > 1.5)
                    npc.velocity.Y -= 0.05f;
                else if (npc.velocity.Y > 0f)
                    npc.velocity.Y += 0.03f;

                if (npc.velocity.Y < -1.5)
                    npc.velocity.Y = -1.5f;
            }
            else if (npc.directionY == 1 && npc.velocity.Y < 1.5)
            {
                npc.velocity.Y += 0.04f;
                if (npc.velocity.Y < -1.5)
                    npc.velocity.Y += 0.05f;
                else if (npc.velocity.Y < 0f)
                    npc.velocity.Y -= 0.03f;

                if (npc.velocity.Y > 1.5)
                    npc.velocity.Y = 1.5f;
            }
        }

        private void RetreatMovement()
        {
            const float incrementX = 0.12f;
            const float incrementY = 0.07f;
            const float thresholdX = 3f;
            const float thresholdY = 1.25f;

            // Stop retreating if one of the two are true:
            // 1) the player is within reach
            // 2) has been retreating for 1000 ticks
            if (CanHitTarget || RetreatTimer > 1000)
            {
                RetreatTimer = 0;
                return;
            }


            // The full logic is that from cycle ticks 1 to 150, it will be going
            // down-left. From cycle ticks 151 to 301, it will go down-right. From 
            // -299 to -151, it will go up-right. From -150 to 0, it will go 
            // in the direction of up-left.

            if (++RetreatDirectionalTimer > 0f)
            {
                // If it's on a positive cycle and it's not going down at a certain
                // speed, try to get to that speed
                if (npc.velocity.Y < thresholdY)
                    npc.velocity.Y += incrementY;
            }
            else if (npc.velocity.Y > -thresholdY)
            {
                // Do the opposite on a negative cycle
                npc.velocity.Y -= incrementY;
            }

            // This logic causes the offset between positive/negative cycles
            // for X from Y
            if (RetreatDirectionalTimer < -150f || RetreatDirectionalTimer > 150f)
            {
                if (npc.velocity.X < thresholdX)
                    npc.velocity.X += incrementX;
            }
            else if (npc.velocity.X > -thresholdX)
            {
                npc.velocity.X -= incrementX;
            }

            if (RetreatDirectionalTimer > 300)
                RetreatDirectionalTimer = -300;
        }

        private void FireProjectile()
        {
            if (CanHitTarget)
            {
                float speedX = PlayerTarget.Center.X - npc.Center.X + Main.rand.Next(-100, 101);
                float speedY = PlayerTarget.Center.Y - npc.Center.Y + Main.rand.Next(-100, 101);
                float speed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                speed = 0.2f / speed;
                speedX *= speed;
                speedY *= speed;
                // TODO: Give this proper projectile
                int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speedX, speedY, ProjectileID.DemonSickle, 21, 0f, Main.myPlayer);
                Main.projectile[proj].timeLeft = 300;
            }
        }

        private void SetProjMax()
        {
            projMax = Main.rand.Next(3, 5);
        }


        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
                for (int k = 0; k < damage / npc.lifeMax * 100; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection, -1f);
                }

                return;
            }

            for (int k = 0; k < 50; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2 * hitDirection, -2f);
            }

            // Couldn't figure out what these gores are, nowhere in source
            //Gore.NewGore(npc.position, npc.velocity, 93);
            //Gore.NewGore(npc.position, npc.velocity, 94);
            //Gore.NewGore(npc.position, npc.velocity, 94);
        }


        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.rotation = npc.velocity.X * 0.1f;
            npc.frameCounter += 0.4;
            // Rounding errors
            if (npc.frameCounter >= 4)
                npc.frameCounter = 0.0;

            // Automatically floors
            int num225 = (int)(npc.frameCounter / 2);
            npc.frame.Y = num225 * frameHeight;
        }
    }
}
