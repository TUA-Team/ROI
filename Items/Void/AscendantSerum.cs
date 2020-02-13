using Terraria;
using Terraria.ID;

namespace ROI.Items.Void
{
    internal abstract class AscendantSerum : VoidItem
    {
        protected override int Affinity => 20;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Serum of Ascendance");
            Tooltip.SetDefault("Grants a 7% increase in critical strike chances when fighting bosses\n" +
                "Use to turn the tables during a boss fight\n" +
                "The drink of warriors bolsters your skills");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 22;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 2;
            item.useTime = 2;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = 3;
            item.value = Item.buyPrice(gold: 1);
        }
    }
}