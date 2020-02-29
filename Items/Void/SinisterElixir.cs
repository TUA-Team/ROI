using Terraria;
using Terraria.ID;

namespace ROI.Items.Void
{
    internal class SinisterElixir : VoidItem
    {
        protected override int Affinity => 30;
        protected override int BuffTime => 5400;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sinister Elixir");
            Tooltip.SetDefault("Greatly increases length of invincibility after taking damage\n" +
                "Decreases the speed of health regeneration");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 32;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 15;
            item.useTime = 15;
            item.UseSound = SoundID.Item2;
            item.maxStack = 30;
            item.rare = 3;
            item.value = Item.buyPrice(gold: 1);
        }
    }
}