using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    public class WastelandWaste : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(30, 184, 175));
            drop = ModContent.ItemType<Items.WastelandWaste>();
            TileID.Sets.ChecksForMerge[Type] = true;
        }
    }
}
