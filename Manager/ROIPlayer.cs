using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Manager
{
    internal class ROIPlayer : ModPlayer
    {
        public List<LoreEntry> loreEntries;

        public override void Initialize()
        {
            loreEntries = new List<LoreEntry>();
        }

        public override void Load(TagCompound tag)
        {
            IList<short> list = tag.GetList<short>("loreEntries");
            for (int i = 0; i < list.Count; i++)
            {
                short entry = list[i];
                loreEntries.Add(new LoreEntry(entry));
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                [nameof(loreEntries)] = loreEntries.Select(x => x.ID)
            };
        }
    }
}
