/*using Microsoft.Xna.Framework;
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
            StrangePresenceDebuff = tag.GetBool(nameof(StrangePresenceDebuff));

            List<TagCompound> npcTags = tag.GetList<TagCompound>(nameof(ISaveableEntity)) as List<TagCompound>;

            foreach (TagCompound npcTag in npcTags)
            {
                Vector2 position = npcTag.Get<Vector2>(nameof(NPC.position));
                if (mod.GetNPC(npcTag.GetString(nameof(NPC.modNPC.Name))).npc is NPC npc)
                {
                    int index = NPC.NewNPC((int)position.X, (int)position.Y, npc.type);

                    if (Main.npc[index].modNPC is ISaveableEntity saveable)
                    {
                        saveable.Load(npcTag);

                        if (saveable.SaveHP && npcTag.ContainsKey(nameof(NPC.life)))
                            Main.npc[index].life = tag.GetAsInt(nameof(NPC.life));
                    }
                }
            }
        }
    }
}
*/