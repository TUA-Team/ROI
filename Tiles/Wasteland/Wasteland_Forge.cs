using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ROI.Tiles.Wasteland
{
    class Wasteland_Forge : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wasteland Forge");
            AddMapEntry(new Color(34,139,34), name);
            disableSmartCursor = true;
            adjTiles = new int[] {TileID.Hellforge, TileID.Furnaces};
            drop = mod.TileType("Wasteland_Forge");
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            g = 1f * 0.8f;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeables.Wasteland.Wasteland_Forge>());
        }
    }
}
