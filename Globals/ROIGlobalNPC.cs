using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Xna.Framework;
using ROI.Manager;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Globals
{
    internal partial class ROIGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        internal bool forcedKill = false;

        public override void SetDefaults(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.MoonLordCore:
                    npc.value = Item.buyPrice(0, 50, 0 , 0);
                    break;
                case NPCID.DukeFishron:
                    npc.value = Item.buyPrice(0, 25, 0, 0); ;
                    break;
                case NPCID.BrainofCthulhu:
                    npc.value = Item.buyPrice(0, 3, 0, 0); ;
                    break;
                case NPCID.QueenBee:
                    npc.value = Item.buyPrice(0, 5, 0, 0); ;
                    break;
                case NPCID.Golem:
                    npc.value = Item.buyPrice(0, 15, 0, 0); ;
                    break;
                case NPCID.CultistBoss:
                    npc.value = Item.buyPrice(0, 20, 0, 0); ;
                    break;
            }
        }

        public override bool PreNPCLoot(NPC npc)
        {
            LogManager.GetLogger("Void logger").Info($"Void Reward : {npc.FullName} - {npc.value} NPC value");

            switch (npc.type)
            {
                case NPCID.EyeofCthulhu:
                    RewardVoidTier(1);
                    break;
                case NPCID.SkeletronHead:
                    RewardVoidTier(2);
                    break;
                case NPCID.WallofFlesh:
                    RewardVoidTier(3);
                    break;
                case NPCID.Plantera:
                    RewardVoidTier(4);
                    break;
                case NPCID.CultistBoss:
                    RewardVoidTier(5);
                    break;
                case NPCID.MoonLordCore:
                    RewardVoidTier(6);
                    break;
            }

            if (npc.boss)
            {
                if (npc.type == NPCID.EaterofWorldsHead && Main.npc.Count(i => i.type == NPCID.EaterofWorldsBody) > 1)
                {
                    return false;
                }
                RewardVoidAffinity(npc);
            }
            return base.PreNPCLoot(npc);
        }

        /*
        public override bool SpecialNPCLoot(NPC npc)
        {

            if (npc.boss)
            {
                if (npc.type == NPCID.EaterofWorldsHead && Main.npc.Count(i => i.type == NPCID.EaterofWorldsBody) > 1)
                {
                    return true;
                }
                RewardVoidAffinity(npc);
                return false;
            }

            return base.SpecialNPCLoot(npc);
        }*/

        public void RewardVoidAffinity(NPC npc)
        {
            foreach (var player in Main.player)
            {
                if (player.name == "")
                {
                    continue;
                }
                player.RewardVoidAffinityTroughNPC(npc);
            }
        }

        private void RewardVoidTier(byte tier)
        {
            foreach (var player in Main.player)
            {
                if (player.name == "")
                {
                    continue;
                }
                player.UnlockVoidTier(tier);
            }
        }

        //TODO:Rewrite this entirely so it spawn from the side of hell instead
        public static void SpawnWoF(On.Terraria.NPC.orig_SpawnWOF orig, Vector2 position)
        {
            if (Main.ActiveWorldFileData.HasCrimson)
            {
                SpawnWastelandCore();
            }
            
            orig(position);
        }

        internal static void SpawnWastelandCore()
        {
            
        }
        
        
       
    }
}
