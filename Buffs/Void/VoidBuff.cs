using Terraria;

namespace ROI.Buffs.Void
{
    internal abstract class VoidBuff
    {
        public VoidBuff() { }

        public abstract void Update(Player player);


        public string Name => GetType().Name;
        public virtual string Texture => $"{GetType().Namespace}/{Name}".Replace('.', '/');
    }
}
