using System;
using Terraria.ModLoader;

namespace API
{
    public sealed class LateLoader<T> where T : BaseLoader, new()
    {
        private bool initialized;
        private T instance;
        private readonly Action<T> onLoad;

        public LateLoader(Mod mod)
        {
            Mod = mod;
        }

        public LateLoader(Mod mod, Action<T> onLoad) : this(mod)
        {
            this.onLoad = onLoad;
        }


        private Mod Mod { get; }

        public T Value
        {
            get
            {
                if (initialized)
                    return instance;

                instance = new T();
                instance.Initialize(Mod);
                onLoad?.Invoke(instance);
                initialized = true;

                return instance;
            }
        }
    }
}
