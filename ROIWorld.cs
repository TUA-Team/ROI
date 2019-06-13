using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Xna.Framework;
using ROI.Buffs.Void;
using ROI.Enums;
using ROI.NPCs.Void.VoidPillar;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI
{
    class ROIWorld : ModWorld
    {
        public bool strangePresenceDebuff { get; internal set; }
        private int pillarSpawningTimer = 60 * 60 * 60;

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            PillarSaving(tag);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            
            PillarLoading(tag);
        }

        private void PillarSaving(TagCompound tag)
        {
            bool isPillarPresent = Main.npc.Any(i => i.modNPC is VoidPillar);
            tag.Add("strangePresenceDebuff", strangePresenceDebuff);
            tag.Add("pillarPresent", isPillarPresent);
            if (isPillarPresent)
            {
                VoidPillar pillar = Main.npc.First(i => i.modNPC is VoidPillar).modNPC as VoidPillar;
                tag.Add("shieldPhase", (byte)pillar.ShieldColor);
                tag.Add("shieldHealth", pillar.ShieldHealth);
                tag.Add("voidPillarX", pillar.npc.position.X);
                tag.Add("voidPillarY", pillar.npc.position.Y);
            }
        }

        private void PillarLoading(TagCompound tag)
        {
            strangePresenceDebuff = tag.GetBool("strangePresenceDebuff");
            if (!tag.GetBool("pillarPresent"))
            {
                return;
            }
            Point position = new Point((int)tag.GetFloat("voidPillarX"), (int)tag.GetFloat("voidPillarY"));
            LogManager.GetLogger("Pillar Loading").Info($"Pillar Position [{position.X}, {position.Y}]");
            int npcID = NPC.NewNPC(position.X, position.Y, mod.NPCType<VoidPillar>());
            VoidPillar newVoidPillar = Main.npc[npcID].modNPC as VoidPillar;
            newVoidPillar.ShieldColor = (PillarShieldColor) tag.GetByte("shieldPhase");
            newVoidPillar.ShieldHealth = tag.GetInt("shieldHealth");
        }

        public override void PreUpdate()
        {
            if (strangePresenceDebuff)
            {
                pillarSpawningTimer--;
                foreach (Player p in Main.player)
                {
                    p.AddBuff(mod.BuffType<PillarPresence>(), 1, true);
                }

                if (pillarSpawningTimer == 0)
                {
                    pillarSpawningTimer = 60 * 60 * 60;
                    strangePresenceDebuff = false;
                }
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(strangePresenceDebuff);
        }

        public override void NetReceive(BinaryReader reader)
        {
            strangePresenceDebuff = reader.ReadBoolean();
        }
    }
}
