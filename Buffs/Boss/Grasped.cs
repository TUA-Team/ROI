using ROI.Players;
using Terraria;

namespace ROI.Buffs.Boss
{
    class Grasped : ROIBuff
    {
        protected Grasped(string displayName) : base("Grasped")
        {
        }

        protected Grasped(string displayName, string description) : base("Grasped", "The heart of the wasteland is pulling you")
        {
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ROIPlayer>().grasped = true;
        }
    }
}
