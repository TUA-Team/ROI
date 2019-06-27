using ROI.ID;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace ROI.Manager
{
    internal class LoreManager : AbstractManager<LoreManager>
    {
        private bool[] loreEntries;

        public void TriggerLoreEntry(short id)
        {
            if (loreEntries[id]) return;
            loreEntries[id] = true;
            GetEntry(id, out string name, out _, out _);
            UIManager.Instance.QueueLore(name);
        }

        public void Load(TagCompound tag)
        {
            loreEntries = new bool[LoreID.Count];
            var list = tag.GetList<short>(nameof(loreEntries));
            for (int i = 0; i < list.Count; i++)
            {
                loreEntries[list[i]] = true;
            }
        }

        public void Save(TagCompound tag)
        {
            var list = new List<short>();
            for (short i = 0; i < LoreID.Count; i++)
            {
                if (loreEntries[i]) list.Add(i);
            }
            tag.Add(nameof(loreEntries), list);
        }

        public void GetEntry(short id, out string Name, out string Desc, out string Author)
        {
            switch (id)
            {
                case LoreID.CopperShortsword:
                    Name = GetLoreLangValue("CopperSword");
                    Desc = GetLoreLangValue("CopperSwordDesc");
                    Author = GetLoreLangValue("TerranFacilities");
                    break;
                default:
                    Name = GetLoreLangValue("Intro");
                    Desc = GetLoreLangValue("IntroDesc");
                    Author = GetLoreLangValue("Team");
                    break;
            }
        }

        private string GetLoreLangValue(string key) => ROIUtils.GetLangValueEasy("Lore" + key);
    }
}
