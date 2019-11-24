using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ROI.Walls.Wasteland
{
    class WastelandDirtWall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(104, 91, 87));
        }
    }

    class WastelandDirtWallSafe : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "ROI/Walls/Wasteland/" + typeof(WastelandDirtWall).Name;
            return true;
        }

        public override void SetDefaults()
        {
            drop = ModContent.ItemType<Items.Placeables.Wasteland.WastelandDirtWall>();
            AddMapEntry(new Color(104, 91, 87));
        }
    }
}
