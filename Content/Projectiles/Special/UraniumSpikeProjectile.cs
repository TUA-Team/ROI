using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Projectiles.Special
{
    public class UraniumSpikeProjectile : ROIProjectile
    {
        public UraniumSpikeProjectile() : base("Uranium Spike", 32, 64) { }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 60;
        }


        public override void AI()
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            bool isLowering = Projectile.timeLeft > 30;
            float percentage = isLowering
                ? (1 - ((Projectile.timeLeft - 30) / 30f))
                : (Projectile.timeLeft / 30f);
            Vector2 pos = Projectile.position - new Vector2(0, 3 * 16 * percentage);
            int srcHeight = (int)(_height * (isLowering
                ? (1 - ((Projectile.timeLeft - 30) / 30f))
                : (Projectile.timeLeft / 30f)));
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("ROI/Content/Projectiles/Special/" + nameof(UraniumSpikeProjectile)).Value,
                pos - Main.screenPosition, new Rectangle(0, 0, _width, srcHeight), Color.White);

            return false;
        }
    }
}
