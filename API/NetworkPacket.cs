﻿using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Localization;
using System.Linq;

namespace API
{
	// https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria/ModLoader/ModPacket.cs
	public abstract class NetworkPacket : BinaryWriter, IHaveId
    {
        #region BinaryWriter overloads

        private byte[] buf;
		private ushort len;
		internal short netID = -1;

		public NetworkPacket(int capacity = 256) : base(new MemoryStream(capacity)) {
			// padding I guess?
			Write((ushort)0);
			Write(MessageID.ModPacket);
			Write(MyId);
		}

		public void Send(object state, int toClient = -1, int ignoreClient = -1) {
			WriteData(state);

			Finish();

			if (Main.netMode == NetmodeID.MultiplayerClient) {
				Netplay.Connection.Socket.AsyncSend(buf, 0, len, SendCallback);

				// LegacyNetDiagnosticsUI.txMsg++;
				// LegacyNetDiagnosticsUI.txData += len;

				if (netID > 0) {
					ModNet.txMsgType[netID]++;
					ModNet.txDataType[netID] += len;
				}
			}
			else if (toClient != -1) {
				Netplay.Clients[toClient].Socket.AsyncSend(buf, 0, len, SendCallback);
			}
			else {
				for (int i = 0; i < 256; i++)
					if (i != ignoreClient && Netplay.Clients[i].IsConnected() && NetMessage.buffer[i].broadcast)
						Netplay.Clients[i].Socket.AsyncSend(buf, 0, len, SendCallback);
			}
		}

		private void SendCallback(object state) { }

		private void Finish() {
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


		public void Init(Mod mod) => Mod = mod;


		public abstract void ReceiveData(BinaryReader reader, int fromWho);

		protected abstract void WriteData(object state);


		public void Link(Action<byte> update) {
			update(MyId);
		}

		public byte MyId { get; set; }

        public Mod Mod { get; private set; }
    }

    public class NetworkPacket<T> : NetworkPacket where T : IHaveState
    {
		public NetworkPacket() {
			StateType = typeof(T);
			StateObjectInstance = (IHaveState)Activator.CreateInstance(StateType);
			Properties = StateType.GetProperties().Select(x => x.Name).ToArray();
        }


        public override void ReceiveData(BinaryReader reader, int fromWho) {
        }

		protected virtual void WriteData(T state) {
		}


		protected sealed override void WriteData(object state) =>
			WriteData((T)state);


		protected Type StateType { get; }
		protected IHaveState StateObjectInstance { get; }

		protected virtual string[] Properties { get; }
    }
}
