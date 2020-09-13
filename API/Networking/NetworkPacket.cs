using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace API.Networking
{
    public abstract class NetworkPacket : BinaryWriter, IHaveId
    {
        private byte[] buf;
        private ushort len;
        private short netID = -1;
        private string mod;

        public NetworkPacket(Stream stream, Encoding encoding, bool leaveOpen)
            : base(stream, encoding, leaveOpen)
        {

        }

        public void Send(object state, int toClient = -1, int ignoreClient = -1)
        {
            if (netID < 0)
                throw new Exception("Cannot get packet for " + mod + " because it has not been synced");

            Write(MessageID.ModPacket);
            if (ModNet.NetModCount < 256)
                Write((byte)netID);
            else
                Write(netID);

            Write(MyId);

            WriteData(state);

            Finish();

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Netplay.Connection.Socket.AsyncSend(buf, 0, len, SendCallback);

                // LegacyNetDiagnosticsUI.txMsg++;
                // LegacyNetDiagnosticsUI.txData += len;

                if (netID != -1)
                {
                    ModNet.txMsgType[netID]++;
                    ModNet.txDataType[netID] += len;
                }
            }
            else if (toClient != -1)
            {
                Netplay.Clients[toClient].Socket.AsyncSend(buf, 0, len, SendCallback);
            }
            else
            {
                for (int i = 0; i < 256; i++)
                    if (i != ignoreClient && Netplay.Clients[i].IsConnected() && NetMessage.buffer[i].broadcast)
                        Netplay.Clients[i].Socket.AsyncSend(buf, 0, len, SendCallback);
            }

            void SendCallback(object obj) { }
        }

        private void Finish()
        {
            if (buf != null)
                return;

            if (OutStream.Position > ushort.MaxValue)
                throw new Exception(Language.GetTextValue("tModLoader.MPPacketTooLarge", OutStream.Position, ushort.MaxValue));

            len = (ushort)OutStream.Position;
            Seek(0, SeekOrigin.Begin);
            Write(len);
            Close();
            buf = ((MemoryStream)OutStream).GetBuffer();
        }

        public abstract void ReceiveData(BinaryReader reader, int fromWho);

        protected abstract void WriteData(object state);

        public virtual void Init(Mod mod)
        {
            netID = (short)mod.GetType().GetField("netID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mod);
            this.mod = mod.Name;
        }

        public void Link(Action<byte> update)
        {
            update(MyId);
        }

        public byte MyId { get; set; }
    }

    // TODO: (med prio) use newtonsoft
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria/ModLoader/ModPacket.cs
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria.ModLoader/ModNet.cs#L407
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria.ModLoader/Mod.cs#L1693
    public class NetworkPacket<T> : NetworkPacket where T : INeedSync, new()
    {
        private IEnumerable<PropertyInfo> properties;
        private INeedSync instance;
        private ShouldSerializeNetworkDataContractResolver resolver;
        private JsonSerializerSettings settings;

        public NetworkPacket() : base(new MemoryStream(261), new UTF8Encoding(false, true), true)
        {
        }


        public override void ReceiveData(BinaryReader reader, int fromWho)
        {
            var state = instance.Identify(reader.ReadInt32());
            var len = reader.ReadInt32();

            JsonConvert.PopulateObject(Encoding.UTF8.GetString(reader.ReadBytes(len)), state);
        }

        protected virtual void WriteData(T state)
        {
            Write(state.Identifier);
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(state, settings));

            Write(bytes.Length);
            Write(bytes, 0, bytes.Length);
        }

        protected sealed override void WriteData(object state) => WriteData((T)state);

        public override void Init(Mod mod)
        {
            base.Init(mod);
            properties = typeof(T).GetType().GetProperties()
                .Where(x => x.GetCustomAttribute<SyncAttribute>() is SyncAttribute attr &&
                attr.PacketType == MyId);
            instance = new T();
            settings = new JsonSerializerSettings
            {
                ContractResolver = new ShouldSerializeNetworkDataContractResolver(MyId)
            };
        }
    }
}
