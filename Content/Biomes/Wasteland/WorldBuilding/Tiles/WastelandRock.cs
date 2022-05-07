using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    public class WastelandRock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileMerge[Type][ModContent.TileType<Wasteland_Dirt")] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandGrass>()] = true;
            AddMapEntry(new Color(68, 74, 100));
            MinPick = 65;
            ItemDrop = ModContent.ItemType<Items.WastelandRock>();
            TileID.Sets.ChecksForMerge[Type] = true;
        }
    }
}