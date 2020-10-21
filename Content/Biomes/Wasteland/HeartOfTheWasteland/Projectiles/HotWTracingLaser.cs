using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.HeartOfTheWasteland.Projectiles
{
    /// <summary>
    /// ai[0] = projectile owner <br \>
    /// ai[1] = projectile mode
    /// localAI[0] = Rotation
    /// </summary>
    internal class HotWTracingLaser : ModProjectile
    {
        public const int BEAM_LENGTH = 2400;
        private const float MOVE_DISTANCE = 60f;

        public float Distance
        {
            get => projectile.localAI[0];
            set => projectile.localAI[0] = value;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
        }

        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.timeLeft = 300;
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.hide = true;
            projectile.aiStyle = 84;
        }

        /*
        public override void AI()
        {
            if (Main.npc[(int) projectile.ai[0]].active && Main.npc[(int) projectile.ai[0]].type == ModContent.NPCType<NPCs.HeartOfTheWasteland.HeartOfTheWasteland>())
            {

            }
            else
            {
                projectile.Kill();
                return;
            }
            switch ((int)projectile.ai[1])
            {
                case 0 :
                    //Follow player logic
                    break;
                case 1:

                    //Spin around central point logic
                    break;
            }
        }
        */
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            NPC owner = Main.npc[(int)projectile.ai[0]];
            Vector2 unit = projectile.velocity;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), owner.Center,
                owner.Center + unit * Distance, 22, ref point);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                writer.Write(projectile.localAI[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                projectile.localAI[0] = reader.ReadSingle();
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            if (target.whoAmI == (int)projectile.localAI[0])
            {
                return true;
            }
            return false;
        }

        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default, int transDist = 50)
        {
            float r = unit.ToRotation() + rotation;

            // Draws the laser 'body'
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                var origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }

            // Draws the laser 'tail'
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

            // Draws the laser 'head'
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Always draw on single player
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center,
                    projectile.velocity, 10, projectile.damage, -1.57f, 1f, 1000f, Color.White, (int)MOVE_DISTANCE);
                return true;
            }
            if ((int)projectile.ai[1] == 1 && (int)projectile.localAI[0] == Main.LocalPlayer.whoAmI)
            {
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center,
                    projectile.velocity, 10, projectile.damage, -1.57f, 1f, 1000f, Color.White, (int)MOVE_DISTANCE);
            }
            if ((int)projectile.ai[1] == 0)
            {
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center,
                    projectile.velocity, 10, projectile.damage, -1.57f, 1f, 1000f, Color.White, (int)MOVE_DISTANCE);
            }
            return true;
        }
    }
}
