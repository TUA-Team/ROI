using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Worlds;
using Terraria;

namespace ROI.Buffs.Void
{
    class PillarPresence : ROIBuff
    {

        protected PillarPresence(string displayName, string description) : base("Strange Presence...", "Something isn't right")
        {
        }

        public override void SetDefaults()
        {
            this.canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            ROIWorld world = mod.GetModWorld<ROIWorld>();
            if (world.StrangePresenceDebuff)
            {
                world.StrangePresenceDebuff = true;
            }
        }
    }
}
