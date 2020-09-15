using API;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader.IO;

namespace ROI.Players
{
    public sealed partial class ROIPlayer
    {
        public override TagCompound Save()
        {
            // TODO: (low prio) can be simplified with linq, probably
            var tag = new TagCompound();

            foreach (var p in GetType().GetProperties())
            {
                if (p.GetCustomAttribute<SaveAttribute>() != null)
                {
                    //Mod.Logger.Debug($"Attempted to seralize property '{p.Name}' with value {p.GetValue(this)}");
                    tag.Add(p.Name, p.GetValue(this));
                }
            }

            return tag;
        }

        public override void Load(TagCompound tag)
        {
            var coll = tag.ToDictionary(x => x.Key, x => x.Value);

            foreach (var p in GetType().GetProperties())
            {
                if (p.GetCustomAttribute<SaveAttribute>() != null)
                {
                    //Mod.Logger.Debug($"Attempted to deseralize property '{p.Name}' with value {p.GetValue(this)}");
                    p.SetValue(this, tag[p.Name]);
                }
            }
        }
    }
}
