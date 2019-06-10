using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Xna.Framework;
using ROI.Enums;
using ROI.NPCs.Void.VoidPillar;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI
{
    class ROIWorld : ModWorld
    {
        private bool strangePresenceDebuff;

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
            LogManager.GetLogger("Pillar Saving").Info("Pillar is present : " + isPillarPresent);
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
            LogManager.GetLogger("Pillar Loading").Info("Pillar is present : " + tag.GetBool("pillarPresent"));
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
    }
}
