using Terraria;
using Terraria.ModLoader;

namespace ROI.Buffs.Void
{
    internal abstract class VoidBuff
    {
        public abstract void SetDefaults();

        public abstract void Update(Player player);


        public ModTranslation DisplayName { get; set; }
        public ModTranslation Description { get; set; }

        public virtual string Texture => (GetType().Namespace + "." + GetType().Name).Replace('.', '/');
    }
}
