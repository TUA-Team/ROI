using API;
using API.Networking;
using API.Users;
using ROI.Loaders;
using ROI.Models.Backgrounds;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace ROI
{
    public sealed partial class ROIMod : Mod
    {
        public CollectionLoader backgroundLoader;
        // internal: see addendum in InterfaceLoader
        internal LateLoader<InterfaceLoader> interfaceLoader;
        // explicit implementation: it's annoying otherwise, the packet system requires too much strongly typed nonsense to be represented by the abstract type
        public LateLoader<NetworkPacketLoader> networkLoader;
        public BaseLoader spawnLoader;
        public LateLoader<UserLoader> userLoader;

        private void InitializeLoaders()
        {
            backgroundLoader = new CollectionLoader<Background>();
            backgroundLoader.Initialize(this);

            interfaceLoader = new LateLoader<InterfaceLoader>(this);

            networkLoader = new LateLoader<NetworkPacketLoader>(this);

            spawnLoader = new SpawnConditionLoader();
            spawnLoader.Initialize(this);

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
            IdHolder.objsByType?.Clear();

            backgroundLoader = null;

            interfaceLoader = null;

            networkLoader = null;

            spawnLoader?.Unload();
            spawnLoader = null;

            userLoader = null;
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            networkLoader.Value.Receive(reader, whoAmI);

        /*
         * 1.4 totally broke this somehow
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
