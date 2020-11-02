using API;
using API.Networking;
using API.Users;
using Microsoft.Xna.Framework;
using ROI.Loaders;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

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
            interfaceLoader = null;

            userLoader = null;

            IdHookLookup.Clear();
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            IdHookLookup<NetworkPacket>.Get(reader.ReadByte()).Receive(reader, whoAmI);

        public override void UpdateUI(GameTime gameTime)
        {
            interfaceLoader.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            interfaceLoader.ModifyInterfaceLayers(layers);
        }
    }
}
