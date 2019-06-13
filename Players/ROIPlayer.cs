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
        }

	    public int MaxVoidAffinity { get; internal set; }

	    public override TagCompound Save()
		{
			return new TagCompound()
			{
				["VoidAffinity"] = voidAffinityAmount,
				["VoidTier"] = VoidTier,
                ["MaxVoidAffinity"] = MaxVoidAffinity,
                ["VoidItemCooldown"] = voidItemCooldown
            };
		}

        public override void Load(TagCompound tag)
		{
			voidAffinityAmount = tag.GetShort("VoidAffinity");
			VoidTier = tag.GetByte("VoidTier");
		    MaxVoidAffinity = tag.GetAsInt("MaxVoidAffinity");
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket(ushort.MaxValue);
			packet.Write(voidAffinityAmount);
			packet.Write(VoidTier);
            packet.Write(voidItemCooldown);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveNetworkData(BinaryReader reader)
		{
            voidAffinityAmount = reader.ReadInt16();
			VoidTier = reader.ReadByte();
		    voidItemCooldown = reader.ReadInt32();
		}

	    public override void PostUpdate()
	    {
	        if (voidItemCooldown != 0)
	        {
	            voidItemCooldown--;
	        }
	    }
	}
}
