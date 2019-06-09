using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROI
{
    static class ROIExtension
    {
        public static void SetAllTrue(this bool[] self)
        {
            for (int i = 0; i < self.Length; i++)
            {
                self[i] = true;
            }
        }

        public static void SetAllFalse(this bool[] self)
        {
            for (int i = 0; i < self.Length; i++)
            {
                self[i] = false;
            }
        }

        public static void AddBuff(this NPC npc, int type, int time1, bool darkMind, bool quiet = false)
        {
            if (darkMind)
            {
                time1 += (int)(time1 * 0.5f);
            }
            npc.AddBuff(type, time1, quiet);
        }

		public static void Kill(this NPC npc)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }
    }
}
