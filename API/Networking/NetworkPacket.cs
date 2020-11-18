using System;
using System.IO;
using Terraria.ModLoader;

namespace API.Networking
{
    public abstract class NetworkPacket : ModType, IHaveId
    {
        protected Action<BinaryWriter> WriteData;

        public NetworkPacket()
        {
            MyId = IdHookLookup<NetworkPacket>.Instances.Count;
        }

        protected NetworkPacket(Action<BinaryWriter> write)
        {
            Instantiate();
            WriteData = write;
        }

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


        protected sealed override void Register()
        {
            IdHookLookup<NetworkPacket>.Register(this);
        }

        protected void Instantiate()
        {
            MyId = IdByType.Get(GetType());
            Load(IdHookLookup<NetworkPacket>.Get(MyId).Mod);
        }

        public int MyId { get; private set; }
    }

    public abstract class NetworkPacket<T> : NetworkPacket
    {
        protected Action<BinaryWriter, T> WriteDataG;

        public NetworkPacket(Action<BinaryWriter, T> write)
        {
            Instantiate();
            WriteDataG = write;
            WriteData = w => write(w, LocalDefault);
        }

        public void Send(T state, int toClient = -1, int ignoreClient = -1)
        {
            var p = Mod.GetPacket();
            p.Write((byte)MyId);
            WriteDataG(p, state);
            p.Send(toClient, ignoreClient);
        }

        protected virtual T LocalDefault => throw new NotImplementedException();
    }
}
