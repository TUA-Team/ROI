using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Items.Interface;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROI
{
    internal partial class ROIUtils
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

        /// <summary>
        /// To increase void tier safely
        /// </summary>
        /// <param name="item"></param>
        /// <param name="tier"></param>
        /// <returns></returns>
        public static bool AddVoidTierToItem(IVoidItem item, int tier)
        {
            if (item.VoidTier == tier - 1)
            {
                item.VoidTier = tier;
                return true;
            }
            return false;
        }

        public static void Swap<T>(ref T first, ref T second)
        {
            var temp = first;
            first = second;
            second = temp;
        }
    }
}
