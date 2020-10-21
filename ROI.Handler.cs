using API;
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
        internal LateLoader<InterfaceLoader> interfaceLoader;
        public LateLoader<UserLoader> userLoader;

        private void InitializeLoaders()
        {
            if (!Main.dedServ)
            {
                interfaceLoader = new LateLoader<InterfaceLoader>(this);
            }

            userLoader = new LateLoader<UserLoader>(this, l => l.ActiveDevelopers = new List<Developer>
                {
                    // TODO: (low prio) create a verification system? probably shouldn't keep these hardcoded, publicly
                    new Developer(76561198062217769, "Dradonhunter11", 0),
                    new Developer(76561197970658570, "2grufs", 0),
                    new Developer(76561193945835208, "DarkPuppey", 0),
                    new Developer(76561193830996047, "Gator", 0),
                    new Developer(76561198098585379, "Chinzilla00", 0),
                    new Developer(76561198265178242, "Demi", 0),
                    new Developer(76561193989806658, "SDF", 0),
                    new Developer(76561198193865502, "Agrair", 0),
                    new Developer(76561198108364775, "HumanGamer", 0),
                    new Developer(76561198046878487, "Webmilio", 0),
                    new Developer(76561198008064465, "Rartrin", 0),
                    new Developer(76561198843721841, "Skeletony", 0)
                });


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
            interfaceLoader.Value.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // see addendum in InterfaceLoader.ModifyInterfaceLayers
            interfaceLoader.Value.ModifyInterfaceLayers(layers, out _);
        }
    }
}
