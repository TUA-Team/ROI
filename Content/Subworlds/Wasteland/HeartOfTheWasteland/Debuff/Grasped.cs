using ROI.Content.Buffs;
using ROI.Players;
using Terraria;

namespace ROI.Content.Subworlds.Wasteland.HeartOfTheWasteland.Debuff
{
    internal class Grasped : ROIBuff
    {
        public Grasped() : base("Grasped", "The heart of the wasteland is pulling you",
            hideTime: true, persistent: true, canBeCleared: false, debuff: true, longerExpertDebuff: true)
        {
        }


        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ROIPlayer>().grasped = true;
            player.buffTime[buffIndex] = 2;
        }
    }
}
