using System.Collections.Generic;

namespace ROI.Manager
{
    internal partial class LoreManager : AbstractManager<LoreManager>
    {
        private Queue<short> loreQueue;

        public override void Initialize()
        {
            loreQueue = new Queue<short>();
        }

        public void TriggerLoreEntry(int plr, short id)
        {
            //Main.player[plr].GetModPlayer<ROIPlayer>().loreEntries.Add(new LoreEntry(id));

            loreQueue.Enqueue(id);
        }
    }
}
