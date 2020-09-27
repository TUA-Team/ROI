using Microsoft.Xna.Framework;
using ROI.Content.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;

namespace ROI.Content.Worlds
{
    partial class ROIWorld
    {
        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound()
            {
                [nameof(StrangePresenceDebuff)] = StrangePresenceDebuff
            };

            List<TagCompound> npcTags = new List<TagCompound>();

            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i] is ISaveableEntity saveable)
                {
                    TagCompound npcTag = new TagCompound();
                    NPC npc = Main.npc[i];

                    npcTag.Add(nameof(NPC.position), npc.position);
                    npcTag.Add(nameof(NPC.modNPC.Name), npc.modNPC.Name);
                    npcTag.Add(nameof(NPC.modNPC.mod), npc.modNPC.mod.Name);

                    if (saveable.SaveHP)
                        npcTag.Add(nameof(NPC.life), npc.life);

                    saveable.Save(npcTag);

                    npcTags.Add(npcTag);
                }
            }

            tag.Add(nameof(ISaveableEntity), npcTags);

            return tag;
        }

        public override void Load(TagCompound tag)
        {
            List<TagCompound> npcs = tag.GetList<TagCompound>(nameof(ISaveableEntity)) as List<TagCompound>;

            foreach (TagCompound currentTag in npcs)
            {
                Vector2 position = currentTag.Get<Vector2>(nameof(NPC.position));
                var npc = mod.GetNPC(currentTag.GetString(nameof(NPC.modNPC.Name)))?.npc;
                if (npc != null)
                {
                    int npcIndex = NPC.NewNPC((int)position.X, (int)position.Y, npc.type);

                    if (Main.npc[npcIndex].modNPC is ISaveableEntity saveable)
                    {
                        saveable.Load(currentTag);

                        if (saveable.SaveHP && currentTag.ContainsKey(nameof(NPC.life)))
                            Main.npc[npcIndex].life = tag.GetAsInt(nameof(NPC.life));
                    }
                }
            }
        }
    }
}
