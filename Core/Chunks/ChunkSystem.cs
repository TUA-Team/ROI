using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using ROI.Utilities.Models;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Core.Chunks
{
    public class ChunkSystem : ModWorld 
    {
        // TODO: move this to a config
        private const int ChunkEnumerateSize = 2;

        private static Dictionary<uint, Chunk> map;
        public static readonly List<ChunkComponent> components = new List<ChunkComponent>();

        public override void Initialize()
        {
            map = new Dictionary<uint, Chunk>();
        }

        public static int RegisterComponent(ChunkComponent component)
        {
            components.Add(component);

            return components.Count - 1;
        }

        public override void PostUpdate()
        {
            foreach (var chunk in FindChunks(Main.LocalPlayer, ChunkEnumerateSize))
            {
                foreach (var component in chunk.GetComponents())
                {
                    component.Update();
                }
            }
        }
        public override void PostDrawTiles()
        {
            SpriteBatch sb = Main.spriteBatch;

            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            foreach (var chunk in FindChunks(Main.LocalPlayer))
            {
                foreach (var component in chunk.GetComponents())
                {
                    component.PostDrawTiles(sb);
                }
            }

            sb.End();
        }
        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();

            foreach (Chunk chunk in map.Values)
            {
                if (chunk.Save() is TagCompound nestedTag)
                {
                    tag[chunk.PackPosition().ToString()] = nestedTag;
                }
            }

            return tag;
        }
        public override void Load(TagCompound tag)
        {
            foreach (var pair in tag)
            {
                uint pos = uint.Parse(pair.Key);
                Chunk chunk = map[pos] = new Chunk((int)pos >> 16, (int)pos & 0xFFFF);
                chunk.Load(tag.GetCompound(pair.Key));
            }
        }

        public static Chunk GetOrCreate(int x, int y)
        {
            uint pos = Chunk.PackPosition(x, y);

            if (!map.TryGetValue(pos, out Chunk chunk))
            {
                chunk = map[pos] = new Chunk(x, y);
            }

            return chunk;
        }
        public static IEnumerable<Chunk> FindChunks(Player player, int areaSize = 0, bool instantiate = false)
        {
            if (player?.active != true)
            {
                return Enumerable.Empty<Chunk>();
            }

            return FindChunks(player.position.ToTileCoordinates(), areaSize, instantiate);
        }
        public static IEnumerable<Chunk> FindChunks(PointS32 tileCenter, int areaSize = 0, bool instantiate = false)
        {
            PointS32 chunkCenter = TileToChunkCoordinates(tileCenter);

            int xStart = chunkCenter.X - areaSize;
            int yStart = chunkCenter.Y - areaSize;
            int xEnd = chunkCenter.X + areaSize;
            int yEnd = chunkCenter.Y + areaSize;

            for (int y = yStart; y < yEnd; y++)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    uint pos = Chunk.PackPosition(x, y);

                    if (!map.TryGetValue(pos, out Chunk chunk))
                    {
                        if (!instantiate)
                            continue;

                        map[pos] = chunk = new Chunk(x, y);
                    }

                    yield return chunk;
                }
            }
        }

        public static int TileToChunkCoordinates(int pos) => pos / Chunk.MaxChunkSize;
        public static PointS32 TileToChunkCoordinates(PointS32 pos) => pos / Chunk.MaxChunkSize;
    }
}