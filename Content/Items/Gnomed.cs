using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items
{
    public class Gnomed : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.value = 2;
            Item.rare = ItemRarityID.White;
            // Item.useStyle = ItemUseStyleID.HoldingUp;
            // Item.useTime = 20;
            // Item.useAnimation = 20;
            // Item.reuseDelay = 50;
            // Item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/GnomedMeme");
        }

        public override void AddRecipes() => CreateRecipe()
                //.AddCondition((Mod as ROIMod).DevRecipeCondition)
                .AddIngredient(ItemID.Wood)
                .Register();
    }
}