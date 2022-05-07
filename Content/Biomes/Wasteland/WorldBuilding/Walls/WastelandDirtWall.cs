using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Walls
{
    public class WastelandDirtWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(104, 91, 87));
        }
    }

    public class WastelandDirtWallSafe : ModWall
    {
        public override string Texture => GetType().Namespace.Replace('.', '/') + "/WastelandDirtWall";

        public override void SetStaticDefaults()
        {
            ItemDrop = ModContent.ItemType<Items.WastelandDirtWall>();
            AddMapEntry(new Color(104, 91, 87));
        }
    }
}