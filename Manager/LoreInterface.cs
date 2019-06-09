using ROI.Manager;
using System.Collections.Generic;
using Terraria;

namespace ROI.Managers
{
    internal class LoreManager : BaseInstanceManager<LoreManager>
    {
        private Queue<short> loreQueue;

        public override void Initialize()
        {
            loreQueue = new Queue<short>();
        }

        public void TriggerLoreEntry(int plr, short id)
        {
            Main.player[plr].GetModPlayer<ROIPlayer>().loreEntries.Add(new LoreEntry(id));

            loreQueue.Enqueue(id);
        }
    }
}
