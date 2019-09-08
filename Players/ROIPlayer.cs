using Microsoft.Xna.Framework;
using ROI.Effects;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
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
        private Dictionary<string, PlayerDeathReason> deathReasonList = new Dictionary<string, PlayerDeathReason>();

        private short _voidAffinityAmount;
        public bool darkMind;
        public int VoidTier { get; internal set; }
        // private short voidExposure;

		//Radiation in precent
	    public float radiationLevel = 0;

        public override void Initialize()
        {
            _voidAffinityAmount = 0;
            darkMind = false;
            // voidExposure = 0;
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
                [nameof(_voidAffinityAmount)] = _voidAffinityAmount,
                [nameof(VoidTier)] = VoidTier,
                [nameof(MaxVoidAffinity)] = MaxVoidAffinity,
                [nameof(voidItemCooldown)] = voidItemCooldown,
                [nameof(VoidHeartHP)] = VoidHeartHP,
                [nameof(MaxVoidHeartStats)] = MaxVoidHeartStats,
				[nameof(radiationLevel)] = radiationLevel
            };
        }

        public override void ResetEffects()
        {
            MaxVoidHeartStatsExtra = MaxVoidHeartStats;
        }

        public override void Load(TagCompound tag)
        {
            _voidAffinityAmount = tag.GetShort(nameof(_voidAffinityAmount));
            VoidTier = tag.GetAsInt(nameof(VoidTier));
            MaxVoidAffinity = tag.GetAsInt(nameof(MaxVoidAffinity));
	        radiationLevel = tag.GetFloat(nameof(radiationLevel));
	        /*deathReasonList.Add("error", new PlayerDeathReason()
	        {
	            SourceCustomReason = player.name + " has got a suspîcious death."
	        });*/
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket(ushort.MaxValue);
            packet.Write(_voidAffinityAmount);
            packet.Write(VoidTier);
            packet.Write(voidItemCooldown);
			packet.Write(radiationLevel);
			packet.Write(_removeRadiationTimer);
			packet.Write(_radiationTimer);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveNetworkData(BinaryReader reader)
        {
            _voidAffinityAmount = reader.ReadInt16();
            VoidTier = reader.ReadByte();
            voidItemCooldown = reader.ReadInt32();
	        radiationLevel = reader.ReadSingle();
	        _removeRadiationTimer = reader.ReadInt32();
	        _radiationTimer = reader.ReadInt32();
        }

        public override void PostUpdate()
        {
            if (voidItemCooldown != 0)
            {
                voidItemCooldown--;
            }

			UpdateRadiation();
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            //player.Hurt(deathReasonList["error"], 0, -1);
            DamageVoidHeart(ref damage);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            //player.Hurt(deathReasonList["error"], 0, -1);
            DamageVoidHeart(ref damage);
        }

        public override void UpdateBiomeVisuals()
        {
            Vector2 playerPosition = Main.LocalPlayer.position / 16;
            if (playerPosition.Y > Main.maxTilesY - 300)
            {


                float percent = ((playerPosition.Y - Main.maxTilesY + 300) / 300);
                if (!Filters.Scene["ROI:UnderworldFilter"].IsActive())
                {
                    Filters.Scene.Activate("ROI:UnderworldFilter", Main.LocalPlayer.Center).GetShader().UseColor(UnderworldDarkness.hell).UseIntensity(percent).UseOpacity(percent);
                }
                Filters.Scene["ROI:UnderworldFilter"].GetShader().UseColor(0f, 0f, 1f).UseIntensity(0.5f).UseOpacity(1f);

            }
            else
            {
                if (Filters.Scene["ROI:UnderworldFilter"].IsActive())
                {
                    Filters.Scene["ROI:UnderworldFilter"].Deactivate();
                }
            }
        }
    }
}
