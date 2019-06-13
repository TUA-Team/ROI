using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROI
{
    static class ROIStaticHelper
    {
        /// <summary>
        /// Credit to jof on this snippet of code, coming from EvenMoreModifiers
        /// </summary>
        /// <param name="buffID"></param>
        /// <returns></returns>
        public static string GetBuffName(int buffID)
        {
            if (buffID >= BuffID.Count)
            {
                return BuffLoader.GetBuff(buffID)?.DisplayName.GetTranslation(LanguageManager.Instance.ActiveCulture) ?? "null";
            }
            return Lang.GetBuffName(buffID);
        }
    }
}
