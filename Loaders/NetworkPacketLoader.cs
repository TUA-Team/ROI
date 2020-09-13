using API;
using API.Networking;
using ROI.Players;
using Terraria.ModLoader;

namespace ROI.Loaders
{
    // not the best, but it allows for a lot simpler utility
    public sealed class NetworkPacketLoader : CollectionLoader
    {
        public override void Initialize(Mod mod)
        {
            base.Initialize(mod);

            // these need to be in the exact order as they are in NetworkPacketID
            Add(new NetworkPacket<ROIPlayer>());
        }
    }

    public static class NetworkPacketID
    {
        public const byte SyncPlayer = 0;
    }
}
