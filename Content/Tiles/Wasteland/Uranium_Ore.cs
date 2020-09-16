using Microsoft.Xna.Framework;
using ROI.Content.Items.Placeables.Wasteland;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Tiles.Wasteland
{
    internal class Uranium_Ore : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandRock>()] = true;
            drop = ModContent.ItemType<Uranium_Chunk>();
            AddMapEntry(new Color(93, 202, 49));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            g = 1;
            r *= 0.1f;
            b *= 0.1f;
            base.ModifyLight(i, j, ref r, ref g, ref b);
        }
    }
}
