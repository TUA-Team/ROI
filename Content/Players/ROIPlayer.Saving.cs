/*using Terraria.ModLoader.IO;

namespace ROI.Players
{
    public sealed partial class ROIPlayer
    {
        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["void"] = SaveVoid()
            };
        }

        public override void Load(TagCompound tag)
        {
            var sub = tag.GetCompound("void");
            if (sub != null)
                LoadVoid(sub);
        }
    }
}
*/