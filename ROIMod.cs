using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROI
{
    // We call this ROIMod instead of ROI since we don't want to have a class with the same name as the namespace its in.
    public sealed partial class ROIMod : Mod
    {
        public static ROIMod Instance { get; private set; }

        public Recipe.Condition DevRecipeCondition { get; private set; }

        public override void Load()
        {
            Instance = this;

            DevRecipeCondition = new Recipe.Condition(NetworkText.FromLiteral("This recipe requires you to be a developer"), 
                r => userLoader.Value.IsDeveloper);

            InitializeLoaders();

            // Utils.GenerateLocalization(this);

            if (!Main.dedServ)
            {
                LoadClient();
            }
        }

        public override void Unload()
        {
            Instance = null;

            UnloadLoaders();

            if (!Main.dedServ)
            {
                UnloadClient();
            }
        }
    }
}