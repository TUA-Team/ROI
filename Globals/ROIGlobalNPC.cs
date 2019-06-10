using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Manager;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Globals
{
    internal partial class ROIGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
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
        }

        public override bool SpecialNPCLoot(NPC npc)
        {
            if (npc.boss)
            {
                if (npc.type == NPCID.EaterofWorldsHead && Main.npc.Count(i => i.type == NPCID.EaterofWorldsBody) > 1)
                {
                    return false;
                }
                RewardVoidAffinity(npc);
            }

            return true;
        }

        public void RewardVoidAffinity(NPC npc)
        {
            if (npc.modNPC.mod.Name == "CalamityMod")
            {
                return;
            }

            foreach (var player in Main.player)
            {
                if (player == null)
                {
                    continue;
                }
                //player.RewardVoidAffinity();
            }
        }

        private void RewardVoidTier(int tier)
        {
            foreach (var player in Main.player)
            {
                if (player == null)
                {
                    continue;
                }
                player.UnlockVoidTier(tier);
            }
        }
    }
}
