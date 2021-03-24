using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Utilities.Models;

namespace ROI.Utilities
{
    public static class DrawUtils
    {
        public static void Draw(this SpriteBatch sb,
            Texture2D texture,
            Vector2 position,
            SimpleDrawSource source,
            Color? color,
            float rotation = 0,
            float scale = 1,
            SpriteEffects effects = SpriteEffects.None,
            float layerDepth = 0)
        {
            Color c = color ?? Color.White;
            sb.Draw(texture, position, source.source, c, rotation, source.origin, scale, effects, layerDepth);
        }
    }
}
