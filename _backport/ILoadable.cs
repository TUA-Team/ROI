using Terraria.ModLoader;

namespace ROI
{
    public interface ILoadable
    {
        void Load(Mod mod);
        void Unload();
    }
}
