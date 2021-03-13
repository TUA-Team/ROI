using ROI.Core.Networking;
using System.IO;
using Terraria.ModLoader;

namespace ROI
{
    partial class ROIMod : Mod
    {
        private void InitializeLoaders()
        {
        }

        private void UnloadLoaders()
        {
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            PacketManager.Instance.Handle(reader, whoAmI);
    }
}
