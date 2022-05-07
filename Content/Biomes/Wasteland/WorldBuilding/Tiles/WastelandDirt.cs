using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    public class WastelandDirt : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandGrass>()] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandRock>()] = true;
            ItemDrop = ModContent.ItemType<Items.WastelandDirt>();
            AddMapEntry(new Color(130, 114, 109));
        }

        // TODO: was this needed?
        /*public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            WorldHelper.TileMergeAttempt(Type, (ushort)ModContent.TileType<WastelandGrass>(), i, j);
            WorldHelper.TileMergeAttempt(Type, (ushort)ModContent.TileType<WastelandRock>(), i, j);

            return true;
        }*/
    }
}