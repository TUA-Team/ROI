using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROI
{
    static class Utils
    {
        /// <summary>
        /// Credit to jof on this snippet of code, coming from EvenMoreModifiers
        /// </summary>
        /// <param name="buffID"></param>
        /// <returns></returns>
        public static string GetBuffName(int buffID) => buffID >= BuffID.Count ?
                (BuffLoader.GetBuff(buffID)?.DisplayName
                    .GetTranslation(LanguageManager.Instance.ActiveCulture) ?? "null")
                : Lang.GetBuffName(buffID);

        public static string GetLangValue(string simplifiedKey)
            => GetLangValue(simplifiedKey);

        public static string GetLangValue(string simplifiedKey, params string[] args)
            => Language.GetTextValue("ROI.Common" + simplifiedKey, args: args);
    }
}
