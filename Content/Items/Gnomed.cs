using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items
{
    public class Gnomed : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 40;
            item.value = 2;
            item.rare = ItemRarityID.White;
            // item.useStyle = ItemUseStyleID.HoldingUp;
            // item.useTime = 20;
            // item.useAnimation = 20;
            // item.reuseDelay = 50;
            // item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/GnomedMeme");
        }

        public override void AddRecipes() => new RecipeBuilder(this)
                //.AddCondition((Mod as ROIMod).DevRecipeCondition)
                .AddIngredient(ItemID.Wood)
                .Register();
    }
}