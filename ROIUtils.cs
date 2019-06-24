using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROI
{
    internal partial class ROIUtils
    {
        public static string GetBuffName(int buffID) => buffID >= BuffID.Count ?
                (BuffLoader.GetBuff(buffID)?.DisplayName
                    .GetTranslation(LanguageManager.Instance.ActiveCulture) ?? "null")
                : Lang.GetBuffName(buffID);

        /// <summary>
        /// Modded only
        /// </summary>
        /// <param name="simplifiedKey"></param>
        /// <returns></returns>
        public static string GetLangValueEasy(string simplifiedKey)
            => GetLangValue(simplifiedKey);

        /// <summary>
        /// Modded only
        /// </summary>
        /// <param name="simplifiedKey"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetLangValue(string simplifiedKey, params object[] args)
            => Language.GetTextValue("ROI.Common" + simplifiedKey, args: args);

        public static T LowClamp<T>(T value, T min) where T : IComparable
        {
            if (value.CompareTo(min) == -1) return min;
            return value;
        }

        public static bool NPCAlive(int type, bool active = true, bool single = false)
        {
            int count = 0;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active == active && Main.npc[i].type == type)
                {
                    if (++count == 2 && single) return false;
                    if (!single) return true;
                }
            }
            return false;
        }
    }
}
