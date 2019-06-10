using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Players
{
	/// <summary>
	/// Use this to store the main player data, otherwise create a partial classà
	/// Web, decide on this one if it's internal or public,
	/// I leave it public in case someone wanna do call in it but I only give them Get accessor
	/// </summary>
	public sealed partial class ROIPlayer : ModPlayer
	{
		private int _voidAffinityAmount = 0;

	    public bool darkMind = false;

	    public int VoidTier { get; internal set; }

	    public int MaxVoidAffinity { get; internal set; }

	    public override TagCompound Save()
		{
			return new TagCompound()
			{
				["VoidAffinity"] = _voidAffinityAmount,
				["VoidTier"] = VoidTier,
                ["MaxVoidAffinity"] = MaxVoidAffinity
                //[nameof(loreEntries)] = loreEntries.Select(x => x.ID)
            };
		}

        public List<LoreEntry> loreEntries;

        public override void Initialize()
        {
            loreEntries = new List<LoreEntry>();
        }

        public override void Load(TagCompound tag)
		{
			_voidAffinityAmount = tag.GetAsInt("VoidAffinity");
			VoidTier = tag.GetAsInt("VoidTier");
		    MaxVoidAffinity = tag.GetAsInt("MaxVoidAffinity");
		    //IList<short> list = tag.GetList<short>("loreEntries");
		    //for (int i = 0; i < list.Count; i++)
		    //{
		    //    short entry = list[i];
		    //    loreEntries.Add(new LoreEntry(entry));
		    //}
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket(ushort.MaxValue);
			packet.Write(_voidAffinityAmount);
			packet.Write(VoidTier);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveNetworkData(BinaryReader reader)
		{
			_voidAffinityAmount = reader.ReadInt32();
			VoidTier = reader.ReadInt32();
		}
	}
}
