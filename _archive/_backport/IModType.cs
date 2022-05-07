namespace Terraria.ModLoader
{
    public interface IModType : ILoadable
    {
        Mod Mod { get; }
    }
}
