using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Walls.Wasteland
{
    class Wasteland_Rock_Wall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(34, 27, 43));
        }
    }

    class Wasteland_Rock_Wall_Safe : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "ROI/Walls/Wasteland/Wasteland_Rock_Wall";
            return true;
        }

        public override void SetDefaults()
        {
            drop = mod.ItemType("Wasteland_Rock_Wall");
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(34, 27, 43));
        }
    }
}
