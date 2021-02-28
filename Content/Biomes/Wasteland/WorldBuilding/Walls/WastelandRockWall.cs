using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Walls
{
    public class WastelandRockWall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(34, 27, 43));
        }
    }

    public class WastelandRockWallSafe : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = GetType().Namespace.Replace('.', '/') + "/WastelandRockWall";
            return true;
        }

        public override void SetDefaults()
        {
            drop = ModContent.ItemType<Items.WastelandRockWall>();
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(34, 27, 43));
        }
    }
}