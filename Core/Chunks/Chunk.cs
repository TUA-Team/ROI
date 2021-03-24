using ROI.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Core.Chunks
{
    public class Chunk
    {
        public const int MaxChunkSize = 64;

        public readonly PointS32 Position;
        public readonly Rect32 TileRectangle;

        private readonly ChunkComponent[] components;

        public Chunk(int x, int y)
        {
            Position = new PointS32(x, y);

            int tileX = x * MaxChunkSize;
            int tileY = y * MaxChunkSize;

            TileRectangle = new Rect32(
                tileX,
                tileY,
                Math.Min(Main.maxTilesX, tileX + MaxChunkSize) - tileX,
                Math.Min(Main.maxTilesY, tileY + MaxChunkSize) - tileY
            );

            components = ChunkSystem.components
                .Select(c => c.Clone(this))
                .ToArray();

            for (int i = 0; i < components.Length; i++)
            {
                components[i].OnInit();
            }
        }

        public T GetComponent<T>() where T : ChunkComponent
        {
            return components[ContentInstance<T>.Instance.Id] as T;
        }
        #region Conditional chunk components
        /*public bool TryGetComponent<T>(out T component) where T : ChunkComponent
        {
            return (component = components[ContentInstance<T>.Instance.Id] as T) != null;
        }*/

        /*public T CreateComponent<T>() where T : ChunkComponent
        {
            T baseComponent = ContentInstance<T>.Instance;
            return (components[baseComponent.Id] = baseComponent.Clone(this)) as T;
        }
        public T GetOrCreateComponent<T>() where T : ChunkComponent
        {
            return TryGetComponent(out T component) ? component : CreateComponent<T>();
        }*/
        #endregion

        public IEnumerable<ChunkComponent> GetComponents()
        {
            return components;
            /*for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is ChunkComponent component)
                {
                    yield return component;
                }
            }*/
        }

        public TagCompound Save()
        {
            TagCompound tag = new TagCompound();

            for (int i = 0; i < components.Length; i++)
            {
                ChunkComponent component = components[i];
                if (component.Save() is TagCompound nestedTag)
                {
                    tag[component.Name] = nestedTag;
                }
            }

            return tag.Count > 0 ? tag : null;
        }
        public void Load(TagCompound tag)
        {
            for (int i = 0; i < components.Length; i++)
            {
                ChunkComponent component = components[i];
                if (tag.ContainsKey(component.Name))
                {
                    component.Load(tag.GetCompound(component.Name));
                }
            }
        }

        public uint PackPosition() => PackPosition(Position.X, Position.Y);
        public static uint PackPosition(int x, int y) => (uint)x << 16 | (ushort)y;
    }
}
