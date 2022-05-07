using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items
{
    public class TerraMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/Terra"),
                ModContent.ItemType<TerraMusicBox>(),
                ModContent.TileType<Tiles.TerraMusicBox>());
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.TerraMusicBox>();
            Item.width = 30;
            Item.height = 10;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 100000;
            Item.accessory = true;
        }
    }
}