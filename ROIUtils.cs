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
    }
}
