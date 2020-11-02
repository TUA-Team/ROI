using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace API.Networking
{
    public abstract class ReboundPacket:NetworkPacket
    {
        public override void Receive(BinaryReader reader, int fromWho)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ReceiveRebound(reader, reader.ReadInt32());
            }

            else // if (server)
                Send(-1, fromWho);
        }


        protected override void SendStyle(ModPacket packet, int toClient, int ignoreClient)
        {
            if (Main.dedServ)
                packet.Write(ignoreClient);
            WriteData(packet);
        }


        protected abstract void ReceiveRebound(BinaryReader reader, int originalClient);
    }
}
