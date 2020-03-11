using Terraria.ModLoader;

namespace ROI.Items.Void
{
    internal class SilverCrest : ModItem
    {
        //TODO: give this an actual use

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.value = 3 * 100;
            item.rare = Terraria.ID.ItemRarityID.Blue;
        }
    }
}