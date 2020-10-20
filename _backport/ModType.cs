namespace Terraria.ModLoader
{
    public abstract class ModType : ILoadable
    {
        public Mod Mod { get; private set; }

        public void Load(Mod mod)
        {
            Mod = mod;
            Register();
        }

        protected abstract void Register();

        public virtual void Unload() { }
    }
}
