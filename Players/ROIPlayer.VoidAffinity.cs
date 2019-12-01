using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        public int voidAffinity;
        public int maxVoidAffinity;

        public int lunarAmassMax = 0;
        public int lunarAmassMin = 0;

        private void VAInit()
        {
            voidAffinity = 0;
            maxVoidAffinity = 50;
        }

        private TagCompound VASave() => new TagCompound
            {
                { nameof(voidAffinity), voidAffinity },
                { nameof(maxVoidAffinity), maxVoidAffinity }
            };

        private void VALoad(TagCompound tag)
        {
            voidAffinity = tag.GetInt(nameof(voidAffinity));
            maxVoidAffinity = tag.GetInt(nameof(maxVoidAffinity));
        }

        private void VAUpdate()
        {
            if (Main.dayTime) return;
            if ((Main.time % 1) != 0) return;
            if (Main.netMode == 1) return;
            if (lunarAmassMax == 0) return;
            if (Main.rand.Next(540) == 0)
            {
                if (maxVoidAffinity == 0) maxVoidAffinity = 50;
                voidAffinity += Main.rand.Next(lunarAmassMin, lunarAmassMax);
            }
        }
    }
}
