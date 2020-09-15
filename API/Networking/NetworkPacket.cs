using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace API.Networking
{
    // TODO: (med prio) instead of keeping streams open, instantiate them only on use
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria/ModLoader/ModPacket.cs
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria.ModLoader/ModNet.cs#L407
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria.ModLoader/Mod.cs#L1693
    public abstract class NetworkPacket : BinaryWriter, IHaveId
    {
        private byte[] buf;
        private ushort len;
        private short netID = -1;
        private string mod;

        public NetworkPacket(Encoding encoding) : base(null, encoding, false)
        {
        }

        public void Send(string kind, object state, int toClient = -1, int ignoreClient = -1)
        {
            if (netID < 0)
                throw new Exception("Cannot get packet for " + mod + " because it has not been synced");

            OutStream = new MemoryStream(261);

            Write((ushort)0);
            Write(MessageID.ModPacket);
            if (ModNet.NetModCount < 256)
                Write((byte)netID);
            else
                Write(netID);

            Write(MyId);
            Write(kind);

            WriteData(kind, state);

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

        public abstract void ReceiveData(BinaryReader reader, string kind, int fromWho);

        protected abstract void WriteData(string kind, object state);

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

    public class NetworkPacket<T> : NetworkPacket where T : INeedSync, new()
    {
        private Type type;
        private INeedSync instance;

        public NetworkPacket() : base(new UTF8Encoding(false, true))
        {
        }


        public override void ReceiveData(BinaryReader reader, string kind, int fromWho)
        {
            var state = instance.Identify(reader.ReadInt32());
            reader.PopulateObjectWProperties(type, state, 
                x => x.GetCustomAttribute<SyncKindAttribute>() is var attr && attr.Kind.EqualsIC(kind));
        }

        protected virtual void WriteData(string kind, T state)
        {
            Write(state.Identifier);
            this.SerializeProperties(type, state, 
                x => x.GetCustomAttribute<SyncKindAttribute>() is var attr && attr.Kind.EqualsIC(kind));
        }

        protected sealed override void WriteData(string kind, object state) => WriteData(kind, (T)state);

        public override void Init(Mod mod)
        {
            base.Init(mod);
            type = typeof(T);
            instance = new T();
        }
    }
}
