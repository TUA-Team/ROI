using Terraria.ModLoader.IO;

namespace ROI.Players
{
    public sealed partial class ROIPlayer
    {
        private void SaveVoid(TagCompound tag) {
            tag.Add(nameof(VoidAffinity), VoidAffinity);
            tag.Add(nameof(MaxVoidAffinity), MaxVoidAffinity);

            tag.Add(nameof(VoidItemCooldown), VoidItemCooldown);

            tag.Add(nameof(VoidHeartHP), VoidHeartHP);

            tag.Add(nameof(MaxVoidHearts), MaxVoidHearts);
        }

        private void LoadVoid(TagCompound tag) {
            VoidAffinity = tag.Get<ushort>(nameof(VoidAffinity));
            MaxVoidAffinity = tag.Get<ushort>(nameof(MaxVoidAffinity));

            VoidItemCooldown = tag.GetAsInt(nameof(VoidItemCooldown));

            VoidHeartHP = tag.GetInt(nameof(VoidHeartHP));

            MaxVoidHearts = tag.GetInt(nameof(MaxVoidHearts));
        }
    }
}
