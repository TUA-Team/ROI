using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.Misc
{
    public class IrradiatedKey : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.maxStack = 1;
        }

        public override void AddRecipes() => CreateRecipe()
                .AddTile(TileID.Hellforge)
                .AddIngredient(ItemID.ShadowKey)
                .Register();
    }
}