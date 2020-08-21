using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Tiles.Hell
{
    class Rubitium_Ore : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            drop = mod.ItemType("Rubitium_Chunk");
            AddMapEntry(new Color(93, 202, 49));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            g = 0.1f;
            r *= 1f;
            b *= 0.1f;
            base.ModifyLight(i, j, ref r, ref g, ref b);
        }
    }
}
