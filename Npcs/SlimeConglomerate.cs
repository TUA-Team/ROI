using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ROI.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.NPCs
{
    internal class SlimeConglomerate : ModNPC
    {
        private static readonly IDictionary<int, Color> SlimeCodes = new Dictionary<int, Color> {
            { 1, new Color (0, 80, 255, 100) }, // blue
            {-3, new Color (0, 220, 40, 100) }, // green
            {-4, new Color (250, 30, 90, 90) }, // pinky
            {-6, new Color (0, 0, 0, 50) }, // black
            {-7, new Color (200, 0, 255, 150) }, // purple
            {-8, new Color (255, 30, 0, 100) }, // red
            {-9, new Color (255, 255, 0, 100) } // yellow
        };

        private int count;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Slime");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            aiType = animationType = NPCID.BlueSlime;
            npc.aiStyle = 1;

            npc.width = 24;
            npc.height = 18;

            npc.damage = 12;
            npc.defense = 6;
            npc.lifeMax = 40;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;

            npc.alpha = 175;
            npc.buffImmune[20] = true;
            npc.buffImmune[31] = false;
            npc.knockBackResist *= 0.9f;
            npc.value = 10f;
            npc.color = SlimeCodes[NPCID.BlueSlime];
            if (Main.expertMode) npc.scaleStats();

            count = 0;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * .008f;
        }

        public override void AI()
        {
            if (Main.netMode == 1 || count == 6) return;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC target = Main.npc[i];
                if (target.active && SlimeCodes.TryGetValue(target.type, out var color) && target.getRect().Intersects(npc.getRect()))
                {
                    target.active = false;
                    target.netUpdate = true;
                    npc.life = (int)NumberHelper.ExpCap(npc.life + target.life, 0, 750, 0.001f);
                    npc.lifeMax = (int)NumberHelper.ExpCap(npc.life + target.lifeMax, 0, 750, 0.001f);
                    npc.color = npc.color.Mix(target.color);
                    npc.scale = (npc.scale + target.scale * .4f).ExpCap(0, 5, .25f);
                    npc.width = (int)(24 * npc.scale);
                    var oldHeight = npc.height;
                    npc.height = (int)(18 * npc.scale);
                    npc.position.Y -= npc.height - oldHeight;
                    npc.height--;
                    npc.netUpdate = true;
                    count++;
                }
            }
        }
    }
}