using Microsoft.Xna.Framework;
using ROI.Loaders;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI
{
    public sealed partial class ROIMod : Mod
    {
        // internal: see addendum in InterfaceLoader
        internal InterfaceLoader interfaceLoader;
        public NetworkPacketLoader networkLoader;
        public SpawnConditionLoader spawnLoader;

        private void InitializeLoaders() {
            interfaceLoader = new InterfaceLoader();
            interfaceLoader.Initialize(this);

            networkLoader = new NetworkPacketLoader();
            networkLoader.Initialize(this);

            spawnLoader = new SpawnConditionLoader();
            spawnLoader.Initialize(this);
        }

        private void UnloadHelpers() {
            interfaceLoader = null;

            networkLoader?.Unload();
            networkLoader = null;

            spawnLoader?.Unload();
            spawnLoader = null;
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            networkLoader.HandlePacket(reader, whoAmI);

        public override void UpdateUI(GameTime gameTime) {
            interfaceLoader.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            // see addendum in InterfaceLoader.ModifyInterfaceLayers
            interfaceLoader.ModifyInterfaceLayers(layers, out _);
        }
    }
}
