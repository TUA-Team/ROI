using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Globals
{
    internal partial class ROIGlobalNPC : GlobalNPC
    {
        private Dictionary<short, byte> bossToVoidTier = new Dictionary<short, byte>
        {
            { NPCID.EyeofCthulhu, 1 },
            { NPCID.SkeletronHead, 2 },
            { NPCID.WallofFlesh, 3 },
            { NPCID.Plantera, 4 },
            { NPCID.CultistBoss, 5 },
            { NPCID.MoonLordCore, 6 }
        };

        public override void NPCLoot(NPC npc)
        {
            if (bossToVoidTier.TryGetValue((short)npc.type, out byte tier))
                RewardVoidTier(tier);
        }

        public override bool SpecialNPCLoot(NPC npc)
        {
            if (npc.boss)
            {
                if (npc.type == NPCID.EaterofWorldsHead && eowAlive()) return false;
                RewardVoidAffinity(npc);
            }
            return true;

            bool eowAlive()
            {
                ushort count = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].type == NPCID.EaterofWorldsBody
                        && ++count == 2) return true;
                }
                return false;
            }
        }

        public void RewardVoidAffinity(NPC npc)
        {
            if (npc.modNPC?.mod.Name == "CalamityMod") return;

            for (int i = 0; i < Main.player.Length; i++)
            {
                //Main.player[i]?.RewardVoidAffinity();
            }
        }

        private void RewardVoidTier(byte tier)
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                Main.player[i]?.UnlockVoidTier(tier);
            }
        }
    }
}
