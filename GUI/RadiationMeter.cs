using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ROI.Players;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI.GUI
{
    internal sealed class RadiationMeter : UIState
    {
        public static bool visible = false;

        private readonly Texture2D back;
        private readonly Texture2D front;

        private UIElement panel;

        private float opacity = 0f;

        private ROIPlayer player;

        public RadiationMeter()
        {
            back = ModContent.GetTexture("ROI/Textures/UIElements/Radiation_Meter");
            front = ModContent.GetTexture("ROI/Textures/UIElements/Radiation_Bar");
        }

        public override void OnInitialize()
        {
            panel = new UIElement
            {
                HAlign = .5f,
                VAlign = .36f
            };
            panel.Width.Set(back.Width, 0f);
            panel.Height.Set(back.Height, 0f);

            Width.Set(Main.screenWidth, 0f);
            Height.Set(Main.screenHeight, 0f);
            Append(panel);
        }

        public override void Update(GameTime gameTime)
        {
            Width.Set(Main.screenWidth, 0f);
            Height.Set(Main.screenHeight, 0f);

            player = Main.LocalPlayer.GetModPlayer<ROIPlayer>();

            if (player.radiationLevel > 0 && opacity < 1f)
            {
                opacity += 0.01f;
            }

            if (player.radiationLevel == 0 && opacity > 0f)
            {
                opacity -= 0.01f;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle style = panel.GetDimensions();
            var pos = style.Position() - new Vector2(0, System.Math.Max(player.player.velocity.Y, 0));
            spriteBatch.Draw(back, pos, Color.White * opacity);

            if ((opacity = Utils.Clamp(opacity, 0, 1)) == 0f)
                return;

            Rectangle source = new Rectangle(18, 64 - (int)(52 * player.radiationLevel), 8, (int)(52 * player.radiationLevel));
            spriteBatch.Draw(front, new Vector2(style.X + 18, pos.Y + 64 - (int)(52 * player.radiationLevel)), source, Color.White * opacity);

            if (panel.IsMouseHovering && opacity >= 0f)
            {
                var color = opacity * (1 - player.radiationLevel);
                Main.spriteBatch.DrawString(Main.fontMouseText,
                    (int)System.Math.Ceiling(100 * player.radiationLevel) + "%",
                    Main.MouseScreen - new Vector2(0, 20), new Color(color, color, color, 255));
            }
        }
    }
}