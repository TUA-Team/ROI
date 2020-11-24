/*using ROI.Content.Buffs.Void;
using ROI.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Worlds
{
    public sealed partial class ROIWorld : ModWorld
    {
        public override void PreUpdate()
        {
            if (StrangePresenceDebuff)
            {
                for (int i = 0; i < Main.player.Length; i++)
                    Main.player[i].AddBuff<PillarPresence>(1, true);
            }
        }


        public bool StrangePresenceDebuff { get; internal set; }

        public int PillarSpawningTime { get; private set; }
    }
}*/