using Terraria.ModLoader.IO;

namespace ROI.Players
{
    public sealed partial class ROIPlayer
    {
        public override TagCompound Save() {
            TagCompound tag = new TagCompound();

            SaveVoid(tag);

            return tag;
        }

        public override void Load(TagCompound tag) {
            LoadVoid(tag);
        }
    }
}
