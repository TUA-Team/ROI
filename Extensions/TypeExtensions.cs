using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;

namespace ROI.Extensions
{
    public static class TypeExtensions
    {
        public static string GetTexturePathFromType(this Type type)
        {
            string[] segments = type.Namespace.Split('.');
            return string.Join("/", segments, 1, segments.Length - 1) + '/' + type.Name;
        }

        /// <summary>Finds the appropriate texture based solely on the type and its associated mod.</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(this Type type) => type.GetModFromType().GetTexture(type.GetTexturePathFromType());

        public static Texture2D GetTexture(this Mod mod, Type type) => mod.GetTexture(type.GetTexturePathFromType());

        public static Texture2D GetTexture(this Type type, Mod mod) => mod.GetTexture(type);

        public static Mod GetModFromType(this Type type) => ModLoader.GetMod(type.Namespace.Split('.')[0]);
    }
}