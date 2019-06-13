using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Players;
using Terraria;

namespace ROI.Buffs.Void
{
    class DarkMind : ROIBuff
    {
        protected DarkMind(string displayName, string description) : base("Your mind is getting consummed by the darkness...", "- 50% damage overall\n+50% debuff duration")
        {
        }
        public override void SetDefaults()
        {
            this.canBeCleared = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ROIPlayer>().darkMind = true;
        }
    }
}
