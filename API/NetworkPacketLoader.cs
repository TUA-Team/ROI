using System.IO;

namespace API
{
    public class NetworkPacketLoader : CollectionLoader<NetworkPacket>
    {
        protected override void OnAdd<TUnique>(TUnique element) {
            element.Init(Mod);
        }

        public void HandlePacket(BinaryReader reader, int sender) {
            byte packetType = reader.ReadByte();

            base[packetType].ReceiveData(reader, sender);
        }

        public TUnique Get<TUnique>() where TUnique : NetworkPacket => (TUnique)this[IdHolder<TUnique>.Id];

        internal static void GenerateMethods() {
            
        }
    }
}
