using ROI.API;
using ROI.API.Networking;
using System.IO;
using Terraria.ModLoader;

namespace ROI
{
    partial class ROIMod : Mod
    {
        private void InitializeLoaders()
        {
            SimpleLoadables.Load(this);

            LiquidAPI.LiquidAPI.Autoload(this);
        }

        private void UnloadLoaders()
        {
            PacketManager.Clear();
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            PacketManager.Handle(reader, whoAmI);
    }
}
