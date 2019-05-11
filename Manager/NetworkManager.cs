using ROI.Manager;
using System;
using System.IO;
using ROI.Enums;
using ROI.Players;
using Terraria;

namespace ROI.Network
{
    internal class NetworkManager : BaseInstanceManager<NetworkManager>
    {

        public override void Initialize()
        {
            
        }

        public void ReceivePacket(BinaryReader networkReader, int whoAmI)
        {
            ROINetworkMessage networkMessage = (ROINetworkMessage) networkReader.ReadByte();
            switch (networkMessage)
            {
                case ROINetworkMessage.PlayerData:
                    Main.player[whoAmI].GetModPlayer<ROIPlayer>().ReceiveNetworkData(networkReader);
                    break;
            }
        }
    }
}
