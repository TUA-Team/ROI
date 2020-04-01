using InfinityCore.API.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ROI.Tiles.Wasteland
{
    /// <summary>
    /// To be used in 0.2
    /// </summary>
    internal class WastelandTree : ModTree, ITreeHook
    {
        private float _brightness = 1f;
        private bool _glowing = false;

        public override int DropWood() => ModContent.ItemType<Items.Placeables.Wasteland.Irrawood>();

        public override bool CanDropAcorn() => false;

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame) =>
            ModContent.GetTexture("ROI/Textures/Tree/Wasteland_Tree_Branch");

        public override Texture2D GetTexture() =>
            ModContent.GetTexture("ROI/Textures/Tree/Wasteland_Tree");

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset) =>
            ModContent.GetTexture("ROI/Textures/Tree/Wasteland_Tree_Top");

        public void PostDrawTreeTop(SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle, Vector2 origin)
        {
            if (_brightness >= 1f || _brightness <= 0f)
                _glowing = !_glowing;
            if (_brightness >= 1f)
                _brightness = 1f;

            _brightness -= _glowing ? 0.001f : -0.01f;
            var tex = ModContent.GetTexture("ROI/Textures/Tree/Wasteland_Tree_Top_Glowmask");
            sb.Draw(tex, position, sourceRectangle, Color.White * _brightness, 0f, origin, 1f, SpriteEffects.None, 1f);
        }

        public void PostDrawTreeBranch(SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle, Vector2 origin)
        {
            var tex = ModContent.GetTexture("ROI/Textures/Tree/Wasteland_Tree_Branch");
            sb.Draw(tex, position, sourceRectangle, Color.White, 0, origin, 1, SpriteEffects.None, 0);
        }
    }
}