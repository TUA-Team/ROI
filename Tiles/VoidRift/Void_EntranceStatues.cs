using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ROI.Tiles.VoidRift
{
    class Void_EntranceStatues : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.Height = 26;
            TileObjectData.newTile.Width = 11;
            TileObjectData.newTile.Origin = new Point16(10, 25);
            TileObjectData.newTile.CoordinateHeights = new int[26];
            for (int i = 0; i < TileObjectData.newTile.CoordinateHeights.Length; i++)
            {
                TileObjectData.newTile.CoordinateHeights[i] = 16;
            }
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Void rift entrance statue");
        }

        /// <summary>
        /// To be implemented
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public override bool NewRightClick(int i, int j)
        {
            return base.NewRightClick(i, j);
        }
    }
}
