using API;
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
                if (!type.IsSubclassOf(typeof(ILoadable)))
                    continue;

                if (type.IsAbstract)
                    continue;
                if (type.ContainsGenericParameters)
                    continue;

                var obj = (ILoadable)Activator.CreateInstance(type);
                obj.Load(mod);

                ContentInstance.Register(obj);
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
