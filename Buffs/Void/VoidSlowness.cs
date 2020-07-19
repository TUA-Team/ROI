using Terraria;
using Terraria.ModLoader;

namespace ROI.Buffs.Void
{
    class VoidSlowness : ModBuff
    {
        //protected VoidSlowness(string displayName, string description) : base("Void Slowness", "The void is slowly consuming you\n- 75% movement speed")
        //{
        //}

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Void Slowness");
            Description.SetDefault("You feel like being a snail");
            Main.debuff[Type] = true;
            this.canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 1 - 0.75f;

        }
    }
}
