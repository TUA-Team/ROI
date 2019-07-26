using Terraria.ModLoader.IO;

namespace ROI.Players
{
    public sealed partial class ROIPlayer
    {
        private void SaveVoid(TagCompound tag)
        {
            tag.Add(nameof(VoidAffinity), VoidAffinity);
        }

        private void LoadVoid(TagCompound tag)
        {
            VoidAffinity = tag.Get<ushort>(nameof(VoidAffinity));
        }
    }
}
