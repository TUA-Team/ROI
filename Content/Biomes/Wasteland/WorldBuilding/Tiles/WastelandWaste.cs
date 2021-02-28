using Microsoft.Xna.Framework;
using ROI.Worlds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    public class WastelandWaste : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(30, 184, 175));
            drop = ModContent.ItemType<Items.WastelandWaste>();
            TileID.Sets.ChecksForMerge[Type] = true;
        }

        //Broken?
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            //Following part need to moved into a seperate class
            int tileToSearch = ModContent.TileType<WastelandDirt>();
            GeneralWorldHelper.RegularMerge(i, j);
            GeneralWorldHelper.SpecialTileMerge(i, j, tileToSearch);
            //RegularMerge(i, j);
            return false;
        }
    }
}
