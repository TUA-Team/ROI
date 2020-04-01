using Terraria;

namespace ROI.Buffs.Void
{
    internal abstract class SinisterElixir : VoidBuff
    {
        public override void Update(Player player)
        {
            Main.NewText("Success!!!");
        }
    }
}
