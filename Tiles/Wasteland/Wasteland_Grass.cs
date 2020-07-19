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
            TileID.Sets.GrassSpecial[Type] = true;
            Main.tileMerge[Type][mod.TileType("Wasteland_Dirt")] = true;
            Main.tileMerge[Type][mod.TileType("Wasteland_Rock")] = true;
            AddMapEntry(new Color(127, 125, 87));
            SetModTree(new WastelandTree());
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.tile[i, j - 1].lava())
            {
                Main.tile[i, j].type = (ushort)mod.TileType("Wasteland_Dirt");
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
            Tile tile16 = Main.tile[i, j - 1];
            Tile tile17 = Main.tile[i, j + 1];
            Tile tile10 = Main.tile[i - 1, j];
            Tile tile11 = Main.tile[i + 1, j];
            Tile tile12 = Main.tile[i - 1, j + 1];
            Tile tile13 = Main.tile[i + 1, j + 1];
            Tile tile14 = Main.tile[i - 1, j - 1];
            Tile tile15 = Main.tile[i + 1, j - 1];
            int upLeft = -1;
            int up = -1;
            int upRight = -1;
            int left = -1;
            int right = -1;
            int downLeft = -1;
            int down = -1;
            int downRight = -1;
            if (tile10 != null && tile10.active())
            {
                left = (Main.tileStone[tile10.type] ? 1 : tile10.type);
                if (tile10.slope() == 1 || tile10.slope() == 3)
                    left = -1;
            }

            if (tile11 != null && tile11.active())
            {
                right = (Main.tileStone[tile11.type] ? 1 : tile11.type);
                if (tile11.slope() == 2 || tile11.slope() == 4)
                    right = -1;
            }

            if (tile16 != null && tile16.active())
            {
                up = (Main.tileStone[tile16.type] ? 1 : tile16.type);
                if (tile16.slope() == 3 || tile16.slope() == 4)
                    up = -1;
            }

            if (tile17 != null && tile17.active())
            {
                down = (Main.tileStone[tile17.type] ? 1 : tile17.type);
                if (tile17.slope() == 1 || tile17.slope() == 2)
                    down = -1;
            }

            if (tile14 != null && tile14.active())
                upLeft = (Main.tileStone[tile14.type] ? 1 : tile14.type);

            if (tile15 != null && tile15.active())
                upRight = (Main.tileStone[tile15.type] ? 1 : tile15.type);

            if (tile12 != null && tile12.active())
                downLeft = (Main.tileStone[tile12.type] ? 1 : tile12.type);

            if (tile13 != null && tile13.active())
                downRight = (Main.tileStone[tile13.type] ? 1 : tile13.type);

            WorldGen.TileMergeAttempt(mod.TileType("Wasteland_Dirt"), Type, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
            //ROIWorldHelper.SpecialTileMerge(i, j, mod.TileType("Wasteland_Dirt"));


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
