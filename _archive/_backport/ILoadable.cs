namespace Terraria.ModLoader
{
    public interface ILoadable
    {
        void Load(Mod mod);
        void Unload();
    }
}
