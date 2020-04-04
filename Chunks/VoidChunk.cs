using InfinityCore.API.Chunks;
using InfinityCore.Worlds.Chunk;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Chunks
{
    internal class VoidChunk : ModChunk
    {
        public bool voidLeak;

        public override bool IsGlobal => base.IsGlobal;

        public override void Draw(SpriteBatch sb, Chunk chunk)
        {
            //TODO: purple dust or something
        }

        public override void Initialize()
        {
            //about 2-3 leaks per small world
            //((200*150)/(4200*1200))*100*100*3 = 178.571428 571428 571428...
            voidLeak = WorldGen.genRand.Next(10000) <= 178;
        }

        public override void Load(TagCompound tag)
        {
            voidLeak = tag.GetBool(nameof(voidLeak));
        }

        public override TagCompound Save()
        {
            return new TagCompound { [nameof(voidLeak)] = voidLeak };
        }
    }
}
