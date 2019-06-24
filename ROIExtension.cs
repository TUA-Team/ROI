using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ROI.Globals;
using ROI.Manager;
using ROI.Players;
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

        public static void UnlockVoidTier(this Player self, byte tier)
        {
            VoidManager.Instance.UnlockTier(self.GetModPlayer<ROIPlayer>(), tier);
        }

        /// <summary>
        /// A method that add void affinity based on how much money an NPC drop
        /// TODO: Add diminishing return on some modded boss
        /// </summary>
        /// <param name="self"></param>
        /// <param name="npc"></param>
        public static void RewardVoidAffinityTroughNPC(this Player self, NPC npc)
        {
            LogManager.GetLogger("Void logger").Info($"Void Reward : {npc.FullName} - {npc.value} NPC value");
            int amount = (int)npc.value / Item.buyPrice(0, 1, 0, 0); ;
            if (amount > 50)
            {
                amount = 50;
            }
            VoidManager.Instance.RewardAffinity(self.GetModPlayer<ROIPlayer>(), amount);
        }

        public static void ForceKill(this NPC npc)
        {
            npc.GetGlobalNPC<ROIGlobalNPC>().forcedKill = true;
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }
    }
}
