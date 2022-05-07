using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ROI.Content.Biomes.Wasteland.Furniture.Tiles
{
    public class WastebrickChandelier : ModTile
    {
        private Asset<Texture2D> flameTexture;

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.StyleWrapLimit = 37;
            TileObjectData.newTile.StyleHorizontal = false;
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wastebrick Chandelier");
            AddMapEntry(new Color(48, 44, 65), name);
            AdjTiles = new int[] { TileID.Torches };

            // Assets
            if (!Main.dedServ)
            {
                flameTexture = ModContent.Request<Texture2D>("ROI/Content/Biomes/Wasteland/Furniture/Tiles/WastebrickChandelierFlame");
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int TileFrameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<Items.WastebrickChandelier>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX < 66)
            {
                Vector3 color = Color.GreenYellow.ToVector3();
                r = color.X;
                g = color.Y;
                b = color.Z;
            }
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short TileFrameY)
        {
            offsetY = 0;
            if (WorldGen.SolidTile(i, j - 1))
            {
                offsetY = 2;
                if (WorldGen.SolidTile(i - 1, j + 1) || WorldGen.SolidTile(i + 1, j + 1))
                {
                    offsetY = 4;
                }
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            ulong seed = (ulong)((long)Main.TileFrameSeed ^ (((long)j << 32) | (uint)i));
            int frameX = Main.tile[i, j].TileFrameX;
            int TileFrameY = Main.tile[i, j].TileFrameY;
            int width = 60;
            int offsetY = 0;
            int height = 60;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            for (int num263 = 0; num263 < 7; num263++)
            {
                float num264 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                float num265 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                spriteBatch.Draw(flameTexture.Value, new Vector2((float)(j * 16 - (int)Main.screenPosition.X) - ((float)width - 16f) / 2f + num264, (float)(i * 16 - (int)Main.screenPosition.Y + offsetY) + num265) + zero, new Microsoft.Xna.Framework.Rectangle(frameX, TileFrameY, width, height), new Microsoft.Xna.Framework.Color(100, 100, 100, 0), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
