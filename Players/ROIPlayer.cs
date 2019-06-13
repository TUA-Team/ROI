using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Players
{
    /// <summary>
    /// Use this to store the main player data, otherwise create a partial class
    /// Web, decide on this one if it's internal or public,
    /// I leave it public in case someone wanna do call in it but I only give them Get accessor
    /// </summary>
    public sealed partial class ROIPlayer : ModPlayer
	{
        private short voidAffinityAmount;
        public bool darkMind;
	    public byte VoidTier { get; internal set; }
        private short voidExposure;

        public override void Initialize()
        {
            voidAffinityAmount = 0;
            darkMind = false;
            voidExposure = 0;

            InitVoid();
        }

        public override TagCompound Save()
		{
			return new TagCompound()
			{
				["voidAffinity"] = voidAffinityAmount,
				["voidTier"] = VoidTier,
                ["voidExposure"] = voidExposure
            };
		}

        public override void Load(TagCompound tag)
		{
			voidAffinityAmount = tag.Get<short>("voidAffinity");
			VoidTier = tag.Get<byte>("voidTier");
            voidExposure = tag.Get<short>("voidExposure");
        }

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket(ushort.MaxValue);
			packet.Write(voidAffinityAmount);
			packet.Write(VoidTier);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveNetworkData(BinaryReader reader)
		{
            voidAffinityAmount = reader.ReadInt16();
            VoidTier = reader.ReadByte();
		}
	}
}
