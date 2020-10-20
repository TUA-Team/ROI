using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    internal class WastelandBrick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ModContent.ItemType<Items.Placeables.Wasteland.Wastebrick>();
            AddMapEntry(new Color(173, 255, 47));
        }
    }
}
