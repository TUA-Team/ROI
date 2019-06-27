using System.Collections.Generic;
using log4net;
using Terraria;
using Terraria.ID;

namespace ROI.Globals
{
    internal partial class ROIGlobalNPC
    {
        public override bool InstancePerEntity => true;
        internal bool forcedKill = false;

        private static readonly Dictionary<short, int> bossValue = new Dictionary<short, int>
        {
            { NPCID.MoonLordCore, 500000 }, // 50 gold
            { NPCID.DukeFishron, 250000 }, // 25 gold
            { NPCID.BrainofCthulhu, 50000 }, // 5 gold
            { NPCID.QueenBee, 30000 }, // 3 gold
            { NPCID.Golem, 150000 }, // 15 gold
            { NPCID.CultistBoss, 200000 } // 20 gold
        };
        private static readonly Dictionary<short, byte> bossTier = new Dictionary<short, byte>
        {
            { NPCID.EyeofCthulhu, 1 },
            { NPCID.SkeletronHead, 2 },
            { NPCID.WallofFlesh, 3 },
            { NPCID.Plantera, 4 },
            { NPCID.CultistBoss, 5 },
            { NPCID.MoonLordCore, 6 }
        };

        private void VoidSetDefaults(NPC npc)
        {
            if (bossValue.TryGetValue((short)npc.type, out int value)) npc.value = value;
        }

        private bool VoidPreNPCLoot(NPC npc)
        {
            LogManager.GetLogger("Void logger").Info($"Void Reward : {npc.FullName} - {npc.value} NPC value");

            if (bossTier.TryGetValue((short)npc.type, out byte tier)) RewardVoidTier(tier);

            if (npc.boss)
            {
                if (npc.type == NPCID.EaterofWorldsHead && eowAlive()) return false;
                RewardVoidAffinity(npc);
            }
            return true;

            bool eowAlive()
            {
                int count = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].type == NPCID.EaterofWorldsBody
                        && ++count == 2) return true;
                }
                return false;
            }
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
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.name == "")
                {
                    continue;
                }
                player.RewardVoidAffinityTroughNPC(npc);
            }
        }

        private void RewardVoidTier(byte tier)
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.name == "")
                {
                    continue;
                }
                player.UnlockVoidTier(tier);
            }
        }
    }
}
