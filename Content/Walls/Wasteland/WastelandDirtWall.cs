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
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "ROI/Content/Walls/Wasteland/WastelandDirtWall";
            return true;
        }

        public override void SetDefaults()
        {
            drop = ModContent.ItemType<Items.Placeables.Wasteland.WastelandDirtWall>();
            AddMapEntry(new Color(104, 91, 87));
        }
    }
}