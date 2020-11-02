using API;

namespace Terraria.ModLoader
{
    public abstract class ModType : IModType
    {
        public Mod Mod { get; private set; }

        public void Load(Mod mod)
        {
            Instantiate(mod);
            Register();
        }

        protected void Instantiate(Mod mod)
        {
            Mod = mod;
        }

        protected abstract void Register();

        public virtual void Unload() { }
    }
}
