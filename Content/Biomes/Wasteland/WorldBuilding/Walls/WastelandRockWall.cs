using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Walls
{
    public class WastelandRockWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(34, 27, 43));
        }
    }

    public class WastelandRockWallSafe : ModWall
    {
        public override string Texture => GetType().Namespace.Replace('.', '/') + "/WastelandRockWall";

        public override void SetStaticDefaults()
        {
            ItemDrop = ModContent.ItemType<Items.WastelandRockWall>();
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(34, 27, 43));
        }
    }
}