using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ROI.Content.Subworlds.Wasteland.Furniture.Tiles
{
    internal class WastebrickBookcase : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData newTile = TileObjectData.newTile;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wastebrick Bookcase");
            AddMapEntry(new Color(48, 44, 65), name);
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Bookcases };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Wastebrick_Bookcase>());
        }
    }
}
