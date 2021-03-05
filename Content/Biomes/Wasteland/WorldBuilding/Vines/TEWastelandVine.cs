using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Vines
{
    public class TEWastelandVine : ModTileEntity
    {
        public int anchorID;
        public int segID;


        public override bool ValidTile(int i, int j)
        {
            return Main.tile[i, j].type == ModContent.TileType<WastelandGrass>();
        }


        public override void OnKill()
        {
            WastelandWorld.vineContext.KillVine(anchorID, segID);
        }
    }
}
