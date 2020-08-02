using ROI.Players;
using Terraria;
using Terraria.ID;

namespace ROI.Buffs.Boss
{
    class Horrified : ROIBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Images/Buff_" + BuffID.Horrified;
            return base.Autoload(ref name, ref texture);
        }

        protected Horrified(string displayName) : base("Horrified")
        {
        }

        protected Horrified(string displayName, string description) : base("Horrified", "You have seen something nasty,\nthere is no escape.")
        {
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ROIPlayer>().horrified = true;
        }
    }
}
