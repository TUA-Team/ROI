using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ROI.Content.Walls.Wasteland
{
    internal class WastelandDirtWall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(104, 91, 87));
        }
    }

    internal class WastelandDirtWallSafe : ModWall
    {
        public override string Texture => ContentInstance<WastelandDirtWall>.Instance.Texture;

        public override void SetDefaults()
        {
            drop = ModContent.ItemType<Items.Placeables.Wasteland.WastelandDirtWall>();
            AddMapEntry(new Color(104, 91, 87));
        }
    }
}