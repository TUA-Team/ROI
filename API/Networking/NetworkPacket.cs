using System.IO;
using Terraria.ModLoader;

namespace API.Networking
{
    public abstract class NetworkPacket : ModType, IHaveId
    {
        public void Send(int toClient = -1, int ignoreClient = -1)
        {
            var p = Mod.GetPacket();
            p.Write((byte)MyId);
            SendStyle(p, toClient, ignoreClient);
            p.Send(toClient, ignoreClient);
        }

        protected virtual void SendStyle(ModPacket packet, int toClient, int ignoreClient)
        {
            WriteData(packet);
        }

        public abstract void Receive(BinaryReader reader, int fromWho);

        protected abstract void WriteData(BinaryWriter writer);


        protected sealed override void Register()
        {
            MyId = IdHookLookup<NetworkPacket>.Instances.Count;
            IdHookLookup<NetworkPacket>.Register(this);
        }

        public int MyId { get; private set; }


        public static TPacket Get<TPacket>(Mod mod) where TPacket : NetworkPacket, new()
        {
            var packet = new TPacket();
            packet.Instantiate(mod);
            packet.MyId = IdByType<TPacket>.Id;
            return packet;
        }
    }

    public abstract class NetworkPacket<T> : NetworkPacket
    {
        public void Send(T state, int toClient = -1, int ignoreClient = -1)
        {
            var p = Mod.GetPacket();
            p.Write((byte)MyId);
            WriteData(p, state);
            p.Send(toClient, ignoreClient);
        }


        protected abstract void WriteData(BinaryWriter writer, T state);


        protected sealed override void WriteData(BinaryWriter writer) => WriteData(writer, LocalDefault);

        protected virtual T LocalDefault => throw new System.NotImplementedException(nameof(LocalDefault));
    }
}
