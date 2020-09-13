using API;
using API.Networking;
using API.Users;
using ROI.Loaders;
using ROI.Models.Backgrounds;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    public sealed partial class ROIMod : Mod
    {
        public CollectionLoader<Background> backgroundLoader;
        // internal: see addendum in InterfaceLoader
        internal InterfaceLoader interfaceLoader;
        public CollectionLoader<NetworkPacket> networkLoader;
        public SpawnConditionLoader spawnLoader;
        public UserLoader userLoader;

        private void InitializeLoaders()
        {
            backgroundLoader = new CollectionLoader<Background>();
            backgroundLoader.OnAdd += b => b.Init(this);
            backgroundLoader.Initialize(this);

            if (!Main.dedServ)
            {
                interfaceLoader = new InterfaceLoader();
                interfaceLoader.Initialize(this);
            }

            networkLoader = new CollectionLoader<NetworkPacket>();
            networkLoader.Initialize(this);

            spawnLoader = new SpawnConditionLoader();
            spawnLoader.Initialize(this);

            userLoader = new UserLoader
            {
                ActiveDevelopers = new List<Developer>
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
                }
            };
            userLoader.Initialize(this);
        }

        private void UnloadLoaders()
        {
            IdHolder.objsByType?.Clear();

            backgroundLoader = null;

            interfaceLoader = null;

            networkLoader = null;

            spawnLoader?.Unload();
            spawnLoader = null;

            userLoader = null;
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            networkLoader?[reader.ReadByte()].ReceiveData(reader, whoAmI);

        /*
        public override void UpdateUI(GameTime gameTime)
        {
            interfaceLoader?.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // see addendum in InterfaceLoader.ModifyInterfaceLayers
            interfaceLoader?.ModifyInterfaceLayers(layers, out _);
        }
        */
    }
}
