using System.IO;
using Terraria.ModLoader;

namespace API
{
    public abstract class NetworkPacket : ModType, IHaveId
    {
        public void Send(object state, int toClient = -1, int ignoreClient = -1)
        {
            var p = Mod.GetPacket();
            p.Write((byte)MyId);
            WriteData(p, state);
            p.Send(toClient, ignoreClient);
        }

        public void Receive(BinaryReader reader, int fromWho) => ReceiveData(reader, fromWho);


        protected abstract void ReceiveData(BinaryReader reader, int fromWho);

        protected abstract void WriteData(BinaryWriter writer, object state);


        protected sealed override void Register()
        {
            IdHookLookup<NetworkPacket>.Register(this);
        }

        public int MyId { get; set; }
    }

    public abstract class NetworkPacket<T> : NetworkPacket
    {
        public void Send(T state, int toClient = -1, int ignoreClient = -1) => base.Send(state, toClient, ignoreClient);


        protected abstract void WriteData(BinaryWriter writer, T state);

        protected sealed override void WriteData(BinaryWriter writer, object state) => WriteData(writer, (T)state);
    }
}
