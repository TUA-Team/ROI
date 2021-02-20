using Microsoft.Xna.Framework;
using ROI.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding.Tiles
{
    public sealed class WastelandRock : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileMerge[Type][ModContent.TileType<Wasteland_Dirt")] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandGrass>()] = true;
            AddMapEntry(new Color(68, 74, 100));
            minPick = 65;
            drop = ModContent.ItemType<Items.WastelandRock>();
            TileID.Sets.ChecksForMerge[Type] = true;
        }

        //TODO: (high prio) Broken?
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            // Following part need to moved into a seperate class
            int tileToSearch = ModContent.TileType<WastelandDirt>();
            WorldHelper.RegularMerge(i, j);
            WorldHelper.SpecialTileMerge(i, j, tileToSearch);
            //RegularMerge(i, j);
            return false;
        }
    }
}