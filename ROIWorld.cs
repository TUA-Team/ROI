using System.IO;
using System.Linq;
using log4net;
using Microsoft.Xna.Framework;
using ROI.Buffs.Void;
using ROI.ID;
using ROI.NPCs.Void.VoidPillar;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI
{
    class ROIWorld : ModWorld
    {
        public bool StrangePresenceDebuff { get; internal set; }
        private int pillarSpawningTimer;

        public override void Initialize()
        {
            pillarSpawningTimer = 216000;
        }

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
            tag.Add("strangePresenceDebuff", StrangePresenceDebuff);
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
            StrangePresenceDebuff = tag.GetBool("strangePresenceDebuff");
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
            if (StrangePresenceDebuff)
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    Main.player[i].AddBuff(mod.BuffType<PillarPresence>(), 1, true);
                }

                if (--pillarSpawningTimer == 0)
                {
                    pillarSpawningTimer = 216000;
                    StrangePresenceDebuff = false;
                }
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(StrangePresenceDebuff);
        }

        public override void NetReceive(BinaryReader reader)
        {
            StrangePresenceDebuff = reader.ReadBoolean();
        }
    }
}
