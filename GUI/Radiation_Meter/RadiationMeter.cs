using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Players;
using Terraria;
using Terraria.UI;

namespace ROI.GUI.Radiation_Meter
{
    class RadiationMeter : UIState
    {
        private static readonly Texture2D backTexture = ROIMod.instance.GetTexture("Textures/UIElements/Radiation_Meter");
        private static readonly Texture2D frontTexture = ROIMod.instance.GetTexture("Textures/UIElements/Radiation_Bar");
        private static readonly Texture2D radiationSymbol = ROIMod.instance.GetTexture("Textures/UIElements/Radiation_Symbol");

        private UIElement panel;

        private bool fade = false;
        private float opacity = 0f;

        public override void OnInitialize()
        {
            panel = new UIElement();
            panel.Width.Set(backTexture.Width, 0f);
            panel.Height.Set(backTexture.Height, 0f);
            panel.HAlign = 0.8f;
            panel.Top.Set(80, 0);
            this.Width.Set(Main.screenWidth, 0f);
            this.Height.Set(Main.screenHeight, 0f);
            Append(panel);
        }

        public override void Update(GameTime gameTime)
        {
            this.Width.Set(Main.screenWidth, 0f);
            this.Height.Set(Main.screenHeight, 0f);


        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {

            CalculatedStyle style = panel.GetDimensions();
            spriteBatch.Draw(backTexture, style.Position(), Color.White * opacity);

            ROIPlayer player = Main.LocalPlayer.GetModPlayer<ROIPlayer>();

            if (player.radiationLevel > 0 && opacity < 1f)
            {
                opacity += 0.01f;
            }

            if (player.radiationLevel == 0 && opacity > 0f)
            {
                opacity -= 0.01f;
            }

            if (opacity == 0f)
                return;

            Rectangle source = new Rectangle(18, 64 - (int)(52 * player.radiationLevel), 8, (int)(52 * player.radiationLevel));
            spriteBatch.Draw(frontTexture, new Vector2(style.X + 18, style.Y + 64 - (int)(52 * player.radiationLevel)), source, Color.White * opacity);

            if (panel.IsMouseHovering && opacity >= 0f)
            {
                Main.hoverItemName = (player.radiationLevel * 100) + "%";
            }
        }
    }
}
