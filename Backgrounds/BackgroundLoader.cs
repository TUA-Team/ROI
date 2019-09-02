using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace ROI.Backgrounds
{
    public static class BackgroundLoader
    {
        private static Dictionary<Type, Background> _backgroundsByTypes;


        #region Loading

        internal static void Load()
        {
            _backgroundsByTypes = new Dictionary<Type, Background>();

            List<Assembly> assemblies = new List<Assembly>();

            foreach (Mod mod in ModLoader.Mods)
                if (mod.Code != null)
                    assemblies.Add(mod.Code);

            foreach (Assembly assembly in assemblies)
                foreach (TypeInfo backgroundType in assembly.DefinedTypes.Where(t => t.IsSubclassOf(typeof(Background))))
                    _backgroundsByTypes.Add(backgroundType, Activator.CreateInstance(backgroundType) as Background);
        }

        internal static void Unload()
        {
            _backgroundsByTypes.Clear();
            _backgroundsByTypes = null;
        }

        #endregion


        public static Background GetBackground<T>() where T : Background => GetBackground(typeof(T));

        public static Background GetBackground(Type type)
        {
            if (!_backgroundsByTypes.ContainsKey(type))
                return null;

            return _backgroundsByTypes[type];
        }


        public static int BackgroundCount => _backgroundsByTypes.Count;
    }
}