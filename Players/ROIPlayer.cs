using System.IO;
using Terraria;
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
        // private short voidExposure;

        public override void Initialize()
        {
            voidAffinityAmount = 0;
            darkMind = false;
            // voidExposure = 0;
            InitVoid();
        }

	    public int MaxVoidAffinity { get; internal set; }

	    public int VoidHeartHP { get; internal set; }

        /// <summary>
        /// Original cap is 100, after that you need to use upgrade and and stuff
        /// </summary>
	    public int MaxVoidHeartStats { get; internal set; }

        /// <summary>
        /// For modders, use this one if you wanna add extra health
        /// </summary>
	    public int MaxVoidHeartStatsExtra { get; set; }

	    public override TagCompound Save()
		{
			return new TagCompound()
			{
				[nameof(voidAffinityAmount)] = voidAffinityAmount,
				[nameof(VoidTier)] = VoidTier,
                [nameof(MaxVoidAffinity)] = MaxVoidAffinity,
                [nameof(voidItemCooldown)] = voidItemCooldown,
                [nameof(VoidHeartHP)] = VoidHeartHP,
                [nameof(MaxVoidHeartStats)] = MaxVoidHeartStats
            };
		}

        public override void ResetEffects()
        {
            MaxVoidHeartStatsExtra = MaxVoidHeartStats;
        }

        public override void Load(TagCompound tag)
		{
			voidAffinityAmount = tag.GetShort(nameof(voidAffinityAmount));
			VoidTier = tag.GetByte(nameof(VoidTier));
		    MaxVoidAffinity = tag.GetAsInt(nameof(MaxVoidAffinity));
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

	    public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
	    {
	        return true;
	    }

	    public override bool CanBeHitByProjectile(Projectile proj)
	    {
	        return true;
	    }

	    public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
	    {
	        DamageVoidHeart(ref damage);
	    }

	    public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
	    {
	        DamageVoidHeart(ref damage);
        }
	}
}
