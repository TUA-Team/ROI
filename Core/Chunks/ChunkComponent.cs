using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ROI.Core.Chunks
{
    public abstract class ChunkComponent : ModType
    {
        public int Id { get; private set; }
        public Chunk Chunk { get; private set; }

        protected sealed override void Register()
        {
            Id = ChunkSystem.RegisterComponent(this);
        }

        public ChunkComponent Clone(Chunk chunk)
        {
            ChunkComponent result = (ChunkComponent)MemberwiseClone();
            result.Chunk = chunk;

            return result;
        }

        public virtual void OnInit() { }
        public virtual void Update() { }
        public virtual void PostDrawTiles(SpriteBatch sb) { }
    }
}
