using System;
using Terraria.ModLoader;

namespace API
{
    public sealed class LateLoader<T> where T : BaseLoader, new()
    {
        private bool initialized;
        private T instance;

        public event Action<T> OnLoad;

        public LateLoader(Mod mod)
        {
            Mod = mod;
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
                OnLoad?.Invoke(instance);
                initialized = true;

                return instance;
            }
        }
    }
}
