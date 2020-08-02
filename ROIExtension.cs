using log4net;
using Microsoft.Xna.Framework;
using ROI.Globals;
using ROI.Manager;
using System.Reflection;
using Terraria;
using ROIPlayer = ROI.Players.ROIPlayer;

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

        public static bool IntersectXAxis(this Rectangle rectangle, Rectangle otherRectangle, int x)
        {
            for (int i = rectangle.Y; i < rectangle.Y + rectangle.Height; i++)
            {
                if (rectangle.Contains(x, i))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IntersectYAxis(this Rectangle rectangle, Rectangle otherRectangle, int y)
        {
            for (int i = rectangle.X; i < rectangle.X + rectangle.Width; i++)
            {
                if (rectangle.Contains(i, y)) { return true; }
            }
            return false;
        }

        public static bool IntersectAdvanced(this Rectangle rectangle, Rectangle otherRectangle, bool xAxis, int startingPoint, int searchingLength)
        {
            if (xAxis)
            {
                for (int i = rectangle.Y + startingPoint; i < rectangle.Y + searchingLength; i++)
                {
                    if (rectangle.Contains(i, startingPoint)) { return true; }
                }
            }
            else
            {
                for (int i = rectangle.X + startingPoint; i < rectangle.X + searchingLength; i++)
                {
                    if (rectangle.Contains(startingPoint, i)) { return true; }
                }
            }
            return false;
        }

        public static T GetField<T>(this object parent, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return (T)parent.GetType().GetField(name, flags).GetValue(parent);
        }
    }
}
