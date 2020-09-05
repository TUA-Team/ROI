using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        public static ROIPlayer Get(Player player) => player.GetModPlayer<ROIPlayer>();


        private void UpdatePreviousBuffs()
        {
            PreviousBuffs.Clear();

            for (int i = 0; i < player.CountBuffs(); i++)
                PreviousBuffs.Add(player.buffType[i]);
        }

        private void OnNewBuffDetected(int previousBuffType)
        {
            // TODO: Check if you can just do player.buffType[previousBuffType] instead of iterating through this.
            for (int i = 0; i < player.buffType.Length; i++)
                if (player.buffType[i] == previousBuffType && Main.debuff[player.buffType[i]])
                    player.buffTime[i] = (int)(player.buffTime[previousBuffType] * DebuffDurationMultiplier);
        }


        public override void Initialize()
        {
            PreviousBuffs = new List<int>();
            InitializeVoid();
        }

        public override void OnEnterWorld(Player player)
        {
            List<int> previousBuffs = new List<int>(PreviousBuffs.ToArray());
            UpdatePreviousBuffs();

            for (int i = 0; i < PreviousBuffs.Count; i++)
                if (!previousBuffs.Contains(PreviousBuffs[i]))
                    OnNewBuffDetected(PreviousBuffs[i]);
        }

        public override void ResetEffects()
        {
            ResetEffectsVoid();
        }


        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            // TODO: is this necessary?
            if (fromWho == Main.myPlayer)
                mod.networkHelper.PlayerSync.SendPacketToAllClients(Main.myPlayer, Main.myPlayer, VoidAffinity, VoidTier, VoidItemCooldown);
        }


        public override void PostUpdate()
        {
            PostUpdateVoid();
        }


        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            ModifyHitByNPCVoid(npc, ref damage, ref crit);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            ModifyHitByProjectileVoid(proj, ref damage, ref crit);
        }


#pragma warning disable IDE1006 // Naming Styles
        public new ROIMod mod => base.mod as ROIMod;
#pragma warning restore IDE1006 // Naming Styles
        private List<int> PreviousBuffs { get; set; }
    }
}