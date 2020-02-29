using Terraria;

namespace ROI.Buffs.Void
{
    internal class SinisterElixir : VoidBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sinister Breath");
            Description.SetDefault("Greatly increases length of invincibility after taking damage\n" +
                "Decreases the speed of health regeneration");
        }

        public override void Update(Player player)
        {
            Main.NewText("Success!!!");
        }
    }
}
