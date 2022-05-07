﻿using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.Furniture.Walls
{
    public class WastestoneBrickWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            ItemDrop = ModContent.ItemType<Items.WastestoneBrickWall>();
            ModTranslation wall = CreateMapEntryName();
            wall.SetDefault("Wastestone Brick Wall");
            AddMapEntry(new Color(173, 255, 47), wall);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.01f;
            g = 0.8f;
            b = 0.2f;
        }
    }
}
