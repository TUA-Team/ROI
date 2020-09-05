using ROI.Loaders;
using Terraria.ModLoader;

namespace ROI
{
    public sealed partial class ROIMod : Mod
    {
        public NetworkPacketLoader networkLoader;
        public SpawnConditionLoader spawnLoader;

        private void InitializeHelpers() {
            spawnLoader = new SpawnConditionLoader();
            spawnLoader.Initialize(this);

            networkLoader = new NetworkPacketLoader();
            networkLoader.Initialize(this);
        }

        private void UnloadHelpers() {
            spawnLoader?.Unload();
            spawnLoader = null;

            networkLoader?.Unload();
            networkLoader = null;
        }
    }
}
