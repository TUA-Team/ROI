using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace ROI.Content.Projectiles.Special
{
    public class UraniumSpikeProjectile : ROIProjectile
    {
        public UraniumSpikeProjectile() : base("Uranium Spike", 32, 64) { }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.timeLeft = 60;
        }


        public override void AI()
        {

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            bool isLowering = projectile.timeLeft > 30;
            float percentage = isLowering
                ? (1 - ((projectile.timeLeft - 30) / 30f))
                : (projectile.timeLeft / 30f);
            Vector2 pos = projectile.position - new Vector2(0, 3 * 16 * percentage);
            int srcHeight = (int)(_height * (isLowering
                ? (1 - ((projectile.timeLeft - 30) / 30f))
                : (projectile.timeLeft / 30f)));
            spriteBatch.Draw(mod.GetTexture("Content/Projectiles/Special/" + nameof(UraniumSpikeProjectile)),
                pos - Main.screenPosition, new Rectangle(0, 0, _width, srcHeight), Color.White);

            return false;
        }
    }
}
