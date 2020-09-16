using System;
using System.IO;
using System.Linq;
using Terraria.ModLoader;

namespace API.Networking
{
    public sealed class NetworkPacketLoader : CollectionLoader
    {
        protected override void OnInitialize(Mod mod)
        {
            OnAdd += element => IdHolder.Register(element);

            var packetType = typeof(NetworkPacket<>);
            var syncType = typeof(INeedSync);

            foreach (var t in mod.Code.DefinedTypes.Where(t => syncType.IsAssignableFrom(t)))
            {
                if (t.IsAbstract) continue;
                if (t.ContainsGenericParameters) continue;
                if (t.IsEquivalentTo(syncType)) continue;

                Add((IHaveId)Activator.CreateInstance(packetType.MakeGenericType(t)));
            }
        }

        internal void Send<T>(string kind, T state, int toWho, int fromWho) where T : INeedSync, new()
        {
            (this[IdHolder<NetworkPacket<T>>.Id] as NetworkPacket).Send(kind, state, toWho, fromWho);
        }

        internal void Receive(BinaryReader reader, int sender)
        {
            (this[reader.ReadByte()] as NetworkPacket).ReceiveData(reader, reader.ReadString(), sender);
        }
    }
}
