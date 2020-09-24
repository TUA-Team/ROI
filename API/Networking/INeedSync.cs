using Terraria.ModLoader;

namespace API.Networking
{
    public interface INeedSync : ILoadable
    {
        int Identifier { get; }

        INeedSync Identify(int identity);
    }
}
