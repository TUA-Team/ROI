using Microsoft.Xna.Framework;
using ROI.Enums;
using ROI.Players;
using System.IO;
using Terraria;
using Terraria.Localization;

namespace ROI.Manager
{
    internal class NetworkManager : AbstractManager<NetworkManager>
    {
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
            Chat(s, color.R, color.G, color.B, sync);
        }

        public static void Chat(string s, byte colorR = 255, byte colorG = 255, byte colorB = 255, bool sync = true)
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
