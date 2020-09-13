using System;
using System.IO;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace API.Networking
{
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria/ModLoader/ModPacket.cs
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria.ModLoader/ModNet.cs#L407
    // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria.ModLoader/Mod.cs#L1693
    public abstract class NetworkPacket : BinaryWriter, IHaveId
    {
        #region BinaryWriter overloads

        private byte[] buf;
        private ushort len;
        private short netID = -1;
        private string mod;

        public NetworkPacket(int capacity = 261) :
            base(new MemoryStream(capacity), new UTF8Encoding(false, true), true)
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
        }

        private void SendCallback(object state) { }

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

        #endregion


        public abstract void ReceiveData(BinaryReader reader, int fromWho);

        protected abstract void WriteData(object state);


        public void Init(Mod mod)
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
}
