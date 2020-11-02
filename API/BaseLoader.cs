using Terraria.ModLoader;

namespace API
{
    public abstract class BaseLoader
    {
        public void Initialize(Mod mod)
        {
            Mod = mod;
            OnInitialize();
        }


        protected virtual void OnInitialize()
        {
        }

        public virtual void Unload()
        {
        }


        protected Mod Mod { get; private set; }
    }
}
