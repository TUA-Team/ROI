using Microsoft.Xna.Framework;
using ROI.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    internal class WastelandDirt : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandGrass>()] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandRock>()] = true;
            drop = ModContent.ItemType<Items.WastelandDirt>();
            AddMapEntry(new Color(130, 114, 109));
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            WorldHelper.TileMergeAttempt(Type, (ushort)ModContent.TileType<WastelandGrass>(), i, j);
            WorldHelper.TileMergeAttempt(Type, (ushort)ModContent.TileType<WastelandRock>(), i, j);

            return true;
        }
    }
}