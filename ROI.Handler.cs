using API;
using API.Networking;
using API.Users;
using ROI.Loaders;
using System.IO;
using Terraria.ModLoader;

namespace ROI
{
    partial class ROIMod : Mod
    {
        // internal: see addendum in InterfaceLoader
        internal InterfaceLoader interfaceLoader;
        // TODO: make this somehow verify users properly
        public LateLoader<UserLoader> userLoader;

        private void InitializeLoaders()
        {
            SimpleLoadables.Load(this);

            userLoader = new LateLoader<UserLoader>(this);

            LiquidAPI.LiquidAPI.Autoload(this);
        }

        private void UnloadLoaders()
        {
            interfaceLoader?.Unload();
            interfaceLoader = null;

            userLoader = null;

            IdHookLookup.Clear();

            PacketManager.Clear();
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            PacketManager.Handle(reader, whoAmI);
    }
}
