using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Tiles.Wasteland
{
    class Wasteland_Dirt : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][mod.TileType("Wasteland_Grass")] = true;
            Main.tileMerge[Type][mod.TileType("Wasteland_Rock")] = true;
            drop = mod.ItemType("Wasteland_Dirt");
            AddMapEntry(new Color(130, 114, 109));

        }

        public override void RandomUpdate(int i, int j)
        {
            base.RandomUpdate(i, j);
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            //Following part need to moved into a seperate class
            int up = Main.tile[i, j - 1].type;
            int down = Main.tile[i, j + 1].type;
            int left = Main.tile[i - 1, j].type;
            int right = Main.tile[i + 1, j].type;

            WorldGen.TileMergeAttempt(Type, mod.TileType("Wasteland_Grass"), ref up, ref down, ref left, ref right);
            WorldGen.TileMergeAttempt(Type, mod.TileType("Wasteland_Rock"), ref up, ref down, ref left, ref right);

            return true;
        }
    }
}
