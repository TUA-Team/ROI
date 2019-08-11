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
    class Wasteland_Grass : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.Grass[Type] = true;
            Main.tileMerge[Type][mod.TileType("Wasteland_Dirt")] = true;
            AddMapEntry(new Color(127, 125, 87));
            SetModTree(new WastelandTree());
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.tile[i, j - 1].lava())
            {
                Main.tile[i, j].type = (ushort) mod.TileType("Wasteland_Dirt");
                WorldGen.SquareTileFrame(i, j, true);
                return;
            }

            if (Main.rand.Next(3) == 0)
                WorldGen.SpreadGrass(i, j, mod.TileType("Wasteland_Dirt"), Type);

            if (Main.rand.Next(2) == 0)
            {
                WorldGen.GrowTree(i - 1, j);
            }
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            //Following part need to moved into a seperate class
            int up = Main.tile[i, j - 1].type;
            int down = Main.tile[i, j + 1].type;
            int left = Main.tile[i - 1, j].type;
            int right = Main.tile[i + 1, j].type;
            
            WorldGen.TileMergeAttempt(Type, mod.TileType("Wasteland_Dirt"), ref up, ref down, ref left, ref right);

            return true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.tile[i, j - 1].type == mod.TileType("Wasteland_Sapling") || Main.tile[i, j - 1].type == mod.TileType("Wasteland_Tree"))
            {
                fail = true;
            }


            Main.tile[i, j].type = (ushort)mod.TileType("Wasteland_Dirt");


        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Color color = new Color(152, 208, 113);
            r = color.ToVector3().X;
            g = color.ToVector3().Y;
            b = color.ToVector3().Z;
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return mod.TileType("ExampleSapling");
        }
    }
}
