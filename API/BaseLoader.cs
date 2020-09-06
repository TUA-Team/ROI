using Terraria.ModLoader;

namespace API
{
    public abstract class BaseLoader
    {
        public virtual void Initialize(Mod mod) {
            Mod = mod;
        }

        public virtual void Unload() {

        }


        protected Mod Mod { get; private set; }
    }
}
