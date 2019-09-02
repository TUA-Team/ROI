using System.Collections.Generic;
using ROI.Players;
using ROI.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.NPCs
{
    public sealed partial class ROIGlobalNPC : GlobalNPC
    {
        private static readonly Dictionary<short, int> _bossValues = new Dictionary<short, int>
        {
            { NPCID.MoonLordCore, Item.sellPrice(gold: 50) }, // 50 gold
            { NPCID.DukeFishron, Item.sellPrice(gold: 25) }, // 25 gold
            { NPCID.BrainofCthulhu, Item.sellPrice(gold: 5) }, // 5 gold
            { NPCID.QueenBee, Item.sellPrice(gold: 3) }, // 3 gold
            { NPCID.Golem, Item.sellPrice(gold: 15) }, // 15 gold
            { NPCID.CultistBoss, Item.sellPrice(gold: 20) } // 20 gold
        };

        private static readonly Dictionary<short, VoidTiers> _bossTiers = new Dictionary<short, VoidTiers>
        {
            { NPCID.EyeofCthulhu, VoidTiers.Alpha },
            { NPCID.SkeletronHead, VoidTiers.Beta },
            { NPCID.WallofFlesh, VoidTiers.Gamma },
            { NPCID.Plantera, VoidTiers.Delta },
            { NPCID.CultistBoss, VoidTiers.Epsilon },
            { NPCID.MoonLordCore, VoidTiers.Zeta }
        };


        private void RewardVoidTier(VoidTiers tier)
        {
            for (int i = 0; i < Main.player.Length; i++)
                ROIPlayer.Get(Main.player[i]).UnlockVoidTier(tier);
        }

        private void RewardVoidAffinity(NPC npc)
        {
            for (int i = 0; i < Main.ActivePlayersCount; i++)
                if (Main.player[i].active)
                    ROIPlayer.Get(Main.player[i]).RewardAffinity(npc);
        }


        private bool PreNPCLootVoid(NPC npc)
        {
            bool IsEoWAlive()
            {
                int count = 0;

                for (int i = 0; i < Main.npc.Length; i++)
                    if (Main.npc[i].type == NPCID.EaterofWorldsBody && ++count == 2)
                        return true;

                return false;
            }

            mod.Logger.Info($"Void Reward: {npc.FullName} ;; NPC Value: {npc.value}");

            if (_bossTiers.TryGetValue((short)npc.type, out VoidTiers tier))
                RewardVoidTier(tier);

            if (npc.boss)
            {
                if (npc.type == NPCID.EaterofWorldsHead && IsEoWAlive())
                    return false;

                RewardVoidAffinity(npc);
            }

            return true;
        }

        private void SetDefaultsVoid(NPC npc)
        {
            if (_bossValues.TryGetValue((short)npc.type, out int value))
                npc.value = value;
        }
    }
}
