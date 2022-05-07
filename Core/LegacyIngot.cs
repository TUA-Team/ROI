using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ROI.Core
{
    public abstract class LegacyIngot : ModTile
    {
        public abstract Color MapEntryColor { get; }
        public abstract string MapNameLegend { get; }
        public abstract int IngotDropName { get; }

        public sealed override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            ModTranslation translation = CreateMapEntryName();
            translation.SetDefault(MapNameLegend);
            AddMapEntry(MapEntryColor, translation);
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int TileFrameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, IngotDropName);
        }
    }
}