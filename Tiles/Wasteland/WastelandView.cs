﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ROI.Tiles.Wasteland
{
    class WastelandView : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(2, 2);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.StyleWrapLimit = 27;
            TileObjectData.addTile(Type);
            ModTranslation translation = CreateMapEntryName("ROI.Painting.WastelandView");
            translation.SetDefault("Painting");
            AddMapEntry(new Color(152, 208, 113), translation);
            drop = mod.ItemType("WastelandView");
        }

    }
}