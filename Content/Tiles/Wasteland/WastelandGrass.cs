using Microsoft.Xna.Framework;
using ROI.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Tiles.Wasteland
{
    internal class WastelandGrass : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.Grass[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandDirt>()] = true;
            Main.tileMerge[Type][ModContent.TileType<WastelandGrass>()] = true;
            AddMapEntry(new Color(127, 125, 87));
            // TODO: SetModTree(new WastelandTree());
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.tile[i, j - 1].lava())
            {
                Main.tile[i, j].type = (ushort)ModContent.TileType<WastelandDirt>();
                WorldGen.SquareTileFrame(i, j, true);
                return;
            }

            if (Main.rand.Next(3) == 0)
                WorldGen.SpreadGrass(i, j, ModContent.TileType<WastelandDirt>(), Type);

            if (Main.rand.Next(2) == 0)
            {
                WorldGen.GrowTree(i - 1, j);
            }
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            WorldHelper.TileMergeAttempt(Type, (ushort)ModContent.TileType<WastelandDirt>(), i, j);
            return true;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {

            return true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Main.tile[i, j].type = (ushort)ModContent.TileType<WastelandDirt>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            var color = new Color(152, 208, 113).ToVector3();
            r = color.X;
            g = color.Y;
            b = color.Z;
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<WastelandSapling>();
        }
    }
}