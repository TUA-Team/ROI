﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    public class WastelandBrick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ModContent.ItemType<Items.Wastebrick>();
            AddMapEntry(new Color(173, 255, 47));
        }
    }
}
