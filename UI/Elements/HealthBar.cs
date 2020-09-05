using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI.UI.Elements
{
    public class HealthBar : UIElement
    {
        public delegate bool GetDrawInfoDG(out string name, out string health,
            out Color color, out float progress);

        private readonly Texture2D _bg;
        private readonly Texture2D _fg;

        public HealthBar(Mod mod) {
            // TODO: (very low prio) make it work with any health bar texture, would also include using StyleDims
            _bg = mod.GetTexture("Textures/Elements/HealthBarBG");
            _fg = mod.GetTexture("Textures/Elements/HealthBarFG");
        }


        public event GetDrawInfoDG GetDrawInfo;

        public override void Draw(SpriteBatch sb) {
            // afaik Draw is singlethreaded, because it would be dumb otherwise
            // however, the official docs for publishing events say that race
            // conditions are possible here, just something to note - Agrair
            if (!GetDrawInfo(out string name, out string health, out Color color, out float barProgress))
                return;

            Vector2 offset = new Vector2(Main.screenWidth / 2, Main.screenHeight - 100);
            if (ModLoader.GetMod("HEROsMod") != null
                || ModLoader.GetMod("CheatSheet") != null) {
                offset.Y -= 50;
            }
            Vector2 textSize = Main.fontDeathText.MeasureString(name) * 0.5f;
            Vector2 healthTextSize = Main.fontDeathText.MeasureString(health) * 0.5f;

            sb.Draw(_bg, 
                new Rectangle((int)offset.X - 250, (int)offset.Y + 41, 20, 41), 
                new Rectangle(0, 0, 20, 41), color * 0.5f);
            sb.Draw(_bg,
                new Rectangle((int)offset.X - 250 + 20, (int)offset.Y + 41, 460, 41),
                new Rectangle(23, 0, 24, 41),
                color * 0.5f);
            sb.Draw(_bg,
                new Rectangle((int)offset.X - 250 + 480, (int)offset.Y + 41, 20, 41),
                new Rectangle(51, 0, 20, 41),
                color * 0.5f);

            int width = (int)(500 * barProgress);
            Rectangle barArea = new Rectangle((int)offset.X - 250 + 2, (int)offset.Y + 41, width, 41);

            sb.Draw(_fg, barArea, new Rectangle(23, 0, 24, 41), color);

            Utils.DrawBorderStringFourWay(sb,
                Main.fontDeathText,
                name,
                (int)offset.X - textSize.X / 2,
                offset.Y,
                Color.Purple,
                Color.MediumPurple,
                Vector2.Zero,
                0.5f);
            Utils.DrawBorderStringFourWay(sb,
                Main.fontDeathText,
                health,
                (int)offset.X - healthTextSize.X / 2,
                offset.Y + healthTextSize.Y + 10,
                Color.LightGray,
                Color.DimGray,
                Vector2.Zero,
                0.5f);
        }
    }
}

