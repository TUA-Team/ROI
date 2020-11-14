using API;
using API.Networking;
using API.Users;
using ROI.Loaders;
using System.IO;
using Terraria;
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
            if (!Main.dedServ)
            {
                interfaceLoader = new InterfaceLoader();
                interfaceLoader.Initialize(this);
            }

            userLoader = new LateLoader<UserLoader>(this);
        }

        private void UnloadLoaders()
        {
            interfaceLoader?.Unload();
            interfaceLoader = null;

            userLoader = null;

            IdHookLookup.Clear();
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            IdHookLookup<NetworkPacket>.Get(reader.ReadByte()).Receive(reader, whoAmI);
    }
}
