using Terraria.ModLoader;

namespace API
{
    public interface IModType:ILoadable
    {
        Mod Mod { get; }
    }
}
