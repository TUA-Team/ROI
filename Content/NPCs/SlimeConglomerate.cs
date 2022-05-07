using Microsoft.Xna.Framework;
using ROI.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ROI.Content.NPCs
{
    public class SlimeConglomerate : ModNPC
    {
        /*private static readonly IDictionary<int, Color> SlimeCodes = new Dictionary<int, Color> {
            { 1, new Color (0, 80, 255, 100) }, // blue
            {-3, new Color (0, 220, 40, 100) }, // green
            {-4, new Color (250, 30, 90, 90) }, // pinky
            {-6, new Color (0, 0, 0, 50) }, // black
            {-7, new Color (200, 0, 255, 150) }, // purple
            {-8, new Color (255, 30, 0, 100) }, // red
            {-9, new Color (255, 255, 0, 100) } // yellow
        };*/

        private static readonly HashSet<int> Slimes = new HashSet<int>
        {
            1,  // blue
            -3, // green
            -4, // pinky
            -6, // black
            -7, // purple
            -8, // red
            -9, // yellow
        };

        private int count;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            AIType = AnimationType = NPCID.BlueSlime;
            NPC.aiStyle = 1;

            NPC.width = 24;
            NPC.height = 18;

            NPC.damage = 12;
            NPC.defense = 6;
            NPC.lifeMax = 40;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.alpha = 175;
            NPC.buffImmune[20] = true;
            NPC.buffImmune[31] = false;
            NPC.knockBackResist *= 0.9f;
            NPC.value = 10f;
            NPC.color = new Color(0, 80, 255, 100);
            // TODO: no clue how to do this
            //if (Main.expertMode)
            //    NPC.ScaleStats(Main.player.Count(x => x.active));

            count = 0;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * .008f;
        }

        public override void AI()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient || count == 6)
                return;

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC target = Main.npc[i];

                if (target.active && Slimes.Contains(target.type) && target.getRect().Intersects(NPC.getRect()))
                {
                    target.active = false;
                    target.netUpdate = true;
                    NPC.life = (int)SpatialUtils.ExpCap(NPC.life + target.life, 0, 750, 0.001f);
                    NPC.lifeMax = (int)SpatialUtils.ExpCap(NPC.life + target.lifeMax, 0, 750, 0.001f);
                    NPC.color = Color.Lerp(NPC.color, target.color, 0.5f);
                    NPC.scale = (NPC.scale + target.scale * .4f).ExpCap(0, 5, .25f);
                    NPC.width = (int)(24 * NPC.scale);
                    var oldHeight = NPC.height;
                    NPC.height = (int)(18 * NPC.scale);
                    NPC.position.Y -= NPC.height - oldHeight;
                    NPC.height--;
                    NPC.netUpdate = true;
                    count++;
                }
            }
        }
    }
}