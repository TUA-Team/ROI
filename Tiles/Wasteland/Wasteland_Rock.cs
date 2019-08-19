using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.Worlds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Tiles.Wasteland
{
    class Wasteland_Rock : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileMerge[Type][mod.TileType("Wasteland_Dirt")] = true;
            Main.tileMerge[Type][mod.TileType("Wasteland_Grass")] = true;
            AddMapEntry(new Color(68, 74, 100));
            minPick = 65;
            drop = mod.ItemType("Wasteland_Rock");
            TileID.Sets.ChecksForMerge[Type] = true;
        }

        //Broken?
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            //Following part need to moved into a seperate class
            int tileToSearch = mod.TileType("Wasteland_Dirt");
            ROIWorldHelper.RegularMerge(i, j);
            ROIWorldHelper.SpecialTileMerge(i, j, tileToSearch);
            //RegularMerge(i, j);
            return false;
        }

    }
}
