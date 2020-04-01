using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Globals
{
    public sealed class RoiGNpc : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<Players.ROIPlayer>().ZoneWasteland)
            {
                pool.Clear();
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant && NPC.downedBoss1)
            {
                shop.item[nextSlot] = new Item();
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Misc.TerraMusicBox>());
                nextSlot++;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            //Item.NewItem(npc.getRect(), );
        }
    }
}