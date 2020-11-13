using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Misc
{
    internal class IrradiatedKey : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 20;
            item.maxStack = 1;
        }

        public override void AddRecipes() => new RecipeBuilder(this)
                .AddTile(TileID.Hellforge)
                .AddIngredient(ItemID.ShadowKey)
                .Register();
    }
}