using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding.Walls
{
    public sealed class WastelandDirtWall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(104, 91, 87));
        }
    }

    public sealed class WastelandDirtWallSafe : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "ROI/Content/Subworlds/Wasteland/WorldBuilding/Walls/WastelandDirtWall";
            return true;
        }

        public override void SetDefaults()
        {
            drop = ModContent.ItemType<Items.WastelandDirtWall>();
            AddMapEntry(new Color(104, 91, 87));
        }
    }
}