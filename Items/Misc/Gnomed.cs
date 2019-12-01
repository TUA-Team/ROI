using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Misc
{
    public class Gnomed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noggin Clontith");
            Tooltip.SetDefault("\"I'm not a gnelf, I'm not a gnoblin, and you've been GNOMED!!\"" +
                "\nA testament to the idiocy of humanity");
        }

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
    }
}