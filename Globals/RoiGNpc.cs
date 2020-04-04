using InfinityCore.Players;
using Microsoft.Xna.Framework;
using ROI.Chunks;
using ROI.Items.Void;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
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

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (!npc.boss && npc.lifeMax > 1 && npc.damage > 0 && !npc.friendly
                && Main.player[npc.FindClosestPlayer(out float dist)].GetCurrentChunk().GetModChunk<VoidChunk>().voidLeak
                && dist < 100
                && Main.rand.Next(5) == 0)
            {
                Dust dust = Main.dust[Dust.NewDust(npc.position, 30, 30, 1, 5 * hitDirection, 0, 0, new Color(255, 255, 255, 1))];
                dust.noGravity = true;
                //no idea what this shader thing actually does, just looked cool in Modders Toolkit - Agrair
                dust.shader = GameShaders.Armor.GetSecondaryShader(56, Main.LocalPlayer);
            }
            return true;
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
            if (!npc.boss && npc.lifeMax > 1 && npc.damage > 0 && !npc.friendly
                && Main.player[npc.FindClosestPlayer(out float dist)].GetCurrentChunk().GetModChunk<VoidChunk>().voidLeak
                && dist < 100
                && Main.rand.Next(4) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<VoidFragment>());
            }
        }
    }
}