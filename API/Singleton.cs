using Terraria.ModLoader;

namespace ROI.API
{
    public abstract class Singleton<T> : ModType where T : Singleton<T>, new()
    {
        private static T instance;


        protected sealed override void Register()
        {
            if (instance != null)
                Mod.Logger.Error($"Singleton `{Name}` has already been loaded.");

            else
                instance = new T();
        }

        public override void Unload()
        {
            if (instance == null)
                Mod.Logger.Error($"Singleton `{Name}` has been destroyed/unloaded already.");

            else if (instance != this)
                Mod.Logger.Error($"Singleton `{Name}` was loaded under a different instance.");

            else
                instance = null;
        }


        public static T Instance => instance;
    }
}
