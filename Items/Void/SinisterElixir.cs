using Terraria;
using Terraria.ID;

namespace ROI.Items.Void
{
    internal abstract class SinisterElixir : VoidItem
    {
        //TODO: more invincibility frames at the cost of regen
        protected override int Affinity => 30;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sinister Elixir");
            Tooltip.SetDefault("Greatly increases length of invincibility after taking damage\n" +
                "Decreases the speed of health regeneration");
        }

        public override void SetDefaults()
        {
            //item.width =
            //item.height =
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 2;
            item.useTime = 2;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = 3;
            item.value = Item.buyPrice(gold: 1);
            //item.buffType = ModContent.BuffType<Buffs.SinisterElixir>();
            item.buffTime = 5400;
        }
    }
}