using Terraria.ModLoader;

namespace API
{
    public abstract class BaseLoader
    {
        public void Initialize(Mod mod)
        {
            OnInitialize(Mod = mod);
        }


        protected virtual void OnInitialize(Mod mod)
        {
        }

        public virtual void Unload()
        {
        }


        protected Mod Mod { get; private set; }
    }
}
