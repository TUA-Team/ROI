using Microsoft.Xna.Framework;
using ROI.Enums;
using System.IO;
using Terraria;
using Terraria.Localization;
using ROIPlayer = ROI.Players.ROIPlayer;

namespace ROI.Manager
{
    internal class NetworkManager : AbstractManager<NetworkManager>
    {

        public override void Initialize()
        {

        }

        public void ReceivePacket(BinaryReader networkReader, int whoAmI)
        {
            ROINetworkMessage networkMessage = (ROINetworkMessage)networkReader.ReadByte();
            switch (networkMessage)
            {
                case ROINetworkMessage.PlayerData:
                    Main.player[whoAmI].GetModPlayer<ROIPlayer>().ReceiveNetworkData(networkReader);
                    break;
            }
        }

        public static void Chat(string s, Color color, bool sync = true)
        {
            Chat(s, (byte)color.R, (byte)color.G, (byte)color.B, sync);
        }

        public static void Chat(string s, byte colorR = (byte)255, byte colorG = (byte)255, byte colorB = (byte)255, bool sync = true)
        {
            switch (Main.netMode)
            {
                case 0:
                case 1:
                    Main.NewText(s, colorR, colorG, colorB);
                    break;
                default:
                    {
                        if (sync && Main.netMode == 2)
                        {
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(s), new Color(colorR, colorG, colorB), -1);
                        }
                        break;
                    }
            }
        }
    }
}
