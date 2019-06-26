using Microsoft.Xna.Framework;
using ROI.ID;
using System.IO;
using Terraria;
using Terraria.Localization;

namespace ROI.Manager
{
    internal class NetworkManager : AbstractManager<NetworkManager>
    {
        public void ReceivePacket(BinaryReader reader, int whoAmI)
        {
            NetworkMessage networkMessage = (NetworkMessage)reader.ReadByte();
            switch (networkMessage)
            {
                case NetworkMessage.PlayerData:
                    if (Main.netMode != 2)
                    {
                        Main.player[whoAmI].GetModPlayer<Players.ROIPlayer>().ReceiveNetworkData(reader);
                    }
                    break;
                case NetworkMessage.FireflyStun:
                    var npc = reader.ReadByte();
                    Main.npc[npc].life -= 15;
                    Main.npc[npc].GetGlobalNPC<NPCs.Globals.EffectNPC>().fireflyStunned = 45;
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
