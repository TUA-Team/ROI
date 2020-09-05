using ROI.Helpers.Networking;
using ROI.Helpers.Spawning;
using Terraria.ModLoader;

namespace ROI
{
    public sealed partial class ROIMod : Mod
    {
        public NetworkPacketHelper networkHelper;
        public SpawnConditionHelper spawnHelper;

        private void InitializeHelpers()
        {
            spawnHelper = new SpawnConditionHelper();
            spawnHelper.Initialize(this);

            networkHelper = new NetworkPacketHelper();
            networkHelper.Initialize(this);
        }

        private void UnloadHelpers()
        {
            spawnHelper?.Unload();
            spawnHelper = null;

            networkHelper?.Unload();
            networkHelper = null;
        }
    }
}
