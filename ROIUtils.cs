using Microsoft.Xna.Framework;
using ROI.Items.Interface;
using System;
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
            => Language.GetTextValue("Mods.ROI.Common." + simplifiedKey, args: args);

        public static void Swap<T>(ref T first, ref T second)
        {
            var temp = first;
            first = second;
            second = temp;
        }

        public static Vector2 RotateVector(Vector2 originalVector, Vector2 centerPoint, double angle)
        {
            Vector2 vector = new Vector2(originalVector.X, originalVector.Y);
            angle = angle * (Math.PI / 180);
            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);

            vector.X = (cosTheta * (vector.X - centerPoint.X) -
                               sinTheta * (vector.Y - centerPoint.Y) + centerPoint.X);
            vector.Y = (sinTheta * (vector.X - centerPoint.X) +
                        cosTheta * (vector.Y - centerPoint.Y) + centerPoint.Y);
            return vector;
        }

        public static ushort TileType<T>() where T : ModTile
            => ModContent.GetInstance<T>().Type;
    }
}
