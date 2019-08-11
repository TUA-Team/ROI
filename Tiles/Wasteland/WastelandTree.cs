using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ROI.Tiles.Wasteland
{
    /// <summary>
    /// To be used in 0.2
    /// </summary>
    internal class WastelandTree : ModTree, IROITreeHook
    {
        private static float _brightness = 1f;
        private static bool _glowing = false;

        private Mod mod => ROIMod.instance;

        public override int DropWood()
        {
            return -1;
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return mod.GetTexture("Textures/Tree/Wasteland_Tree_Branch");
        }

        public override Texture2D GetTexture()
        {
            return mod.GetTexture("Textures/Tree/Wasteland_Tree");
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            return mod.GetTexture("Textures/Tree/Wasteland_Tree_Top");
        }


        public void PostDrawTreeTop(SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle, Vector2 origin)
        {
            if (_brightness >= 1f || _brightness <= 0f)
                _glowing = !_glowing;
            if (_brightness >= 1f)
                _brightness = 1f;


            _brightness -= (_glowing) ? 0.001f : -0.01f;
            sb.Draw(mod.GetTexture("Textures/Tree/Wasteland_Tree_Top_Glowmask"), position, sourceRectangle, Color.White * _brightness, 0f, origin, 1f, SpriteEffects.None, 1f);
        }
    }
}
