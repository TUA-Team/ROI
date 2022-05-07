using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ROI.Content.Biomes.Wasteland.Furniture.Tiles
{
    public class WastebrickBookcase : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wastebrick Bookcase");
            AddMapEntry(new Color(48, 44, 65), name);
            TileObjectData.addTile(Type);
            AdjTiles = new int[] { TileID.Bookcases };
        }

        public override void KillMultiTile(int i, int j, int frameX, int TileFrameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 64, ModContent.ItemType<Items.WastebrickBookcase>());
        }
    }
}
