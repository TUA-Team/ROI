using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ROI
{
    public interface ITreeHook
    {
        void PostDrawTreeTop(SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle, Vector2 origin);
    }
}
