using ROI.Helpers;
using ROI.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.NPCs
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

        private static readonly Dictionary<short, VoidTier> _bossTiers = new Dictionary<short, VoidTier>
        {
            { NPCID.EyeofCthulhu, VoidTier.Alpha },
            { NPCID.SkeletronHead, VoidTier.Beta },
            { NPCID.WallofFlesh, VoidTier.Gamma },
            { NPCID.Plantera, VoidTier.Delta },
            { NPCID.CultistBoss, VoidTier.Epsilon },
            { NPCID.MoonLordCore, VoidTier.Zeta }
        };


        private void RewardVoidTier(VoidTier tier)
        {
            for (int i = 0; i < Main.player.Length; i++)
                ROIPlayer.Get(Main.player[i]).UnlockVoidTier(tier);
        }

        private void RewardVoidAffinity(NPC npc)
        {
            for (int i = 0; i < Main.player.Length; i++)
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

            Mod.Logger.Info($"Void Reward: {npc.FullName} ;; NPC Value: {npc.value}");

            if (_bossTiers.TryGetValue((short)npc.type, out VoidTier tier))
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
