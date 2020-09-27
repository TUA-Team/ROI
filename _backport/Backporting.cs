using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ROI
{
    public static class Backporting
    {
        private static readonly List<ILoadable> loadables = new List<ILoadable>();

        public static void Init(Mod mod)
        {
            foreach (var type in mod.Code.DefinedTypes)
            {
                if (type.IsAbstract) continue;
                if (type.ContainsGenericParameters) continue;

                if (!typeof(ILoadable).IsAssignableFrom(type)) continue;

                var obj = (ILoadable)Activator.CreateInstance(type);
                obj.Load(mod);

                loadables.Add(obj);
            }
        }

        public static void Clear()
        {
            foreach (var loadable in loadables)
            {
                loadable.Unload();
            }
        }
    }
}
