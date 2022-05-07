namespace Terraria.ModLoader
{
    public abstract class ModType : IModType
    {
        public Mod Mod { get; private set; }

        public string Name => GetType().Name;

        public void Load(Mod mod)
        {
            Mod = mod;
            Load();
            Register();
        }

        public virtual void Load() { }

        protected virtual void Register() { }

        public virtual void Unload() { }
    }
}
