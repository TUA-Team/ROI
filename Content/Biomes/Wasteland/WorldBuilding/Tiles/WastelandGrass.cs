using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Core.Verlet.Contexts.Chains;
using ROI.Content.Biomes.Wasteland.WorldBuilding.Vines;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    public class WastelandGrass : ModTile
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

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            // TODO: Make this work with slopes and stuff
            int index = ModContent.GetInstance<TEWastelandVine>().Find(i, j);
            if (index != -1)
            {
                TEWastelandVine te = TileEntity.ByID[index] as TEWastelandVine;
                VerletChainContext ctx = WastelandWorld.vineContext;

                ctx.Update(te);
                ctx.Draw(spriteBatch, te);
            }

            return true;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            var color = new Color(152, 208, 113).ToVector3();
            r = color.X;
            g = color.Y;
            b = color.Z;
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

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<WastelandSapling>();
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Main.tile[i, j].type = (ushort)ModContent.TileType<WastelandDirt>();

            var te = ModContent.GetInstance<TEWastelandVine>();
            if (te.Find(i, j) != -1)
            {
                te.Kill(i, j);
                Dust.NewDust(new Vector2(i * 16, j * 16), 5, 5, DustID.Grass);
                Main.PlaySound(SoundID.Grass, i * 16, j * 16);
            }
        }
    }
}