/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Core.Chunks;
using ROI.Utilities.Models;
using System;
using Terraria;

namespace ROI.Common
{
    // Adapted from Overhaul
    public sealed class ChunkLighting : ChunkComponent
    {
        public static int LightingUpdateFrequency => 10;

        public RenderTarget2D Texture { get; private set; }
        public Surface<Color> Colors { get; private set; }

        public override void Load()
        {
            On.Terraria.Lighting.LightTiles += (On.Terraria.Lighting.orig_LightTiles orig, int firstX, int lastX, int firstY, int lastY) =>
            {
                if (Main.GameUpdateCount % LightingUpdateFrequency == 0)
                {
                    const int Offset = 4;

                    // The boundaries of the screen, in tile coordinates
                    Rect32 tileLoopArea = new Rect32(
                        (int)Math.Floor(Main.screenPosition.X / 16f) - Offset,
                        (int)Math.Floor(Main.screenPosition.Y / 16f) - Offset,

                        (int)Math.Ceiling(Main.screenWidth / 16f) + Offset * 2,
                        (int)Math.Ceiling(Main.screenHeight / 16f) + Offset * 2
                    );

                    // The chunks that intersect with the screen
                    Rect32 chunkLoopArea = new Rect32(
                        ChunkSystem.TileToChunkCoordinates(tileLoopArea.Pos),
                        ChunkSystem.TileToChunkCoordinates(tileLoopArea.Size)
                    );

                    for (int y = chunkLoopArea.Pos.Y; y <= chunkLoopArea.Max.Y; y++)
                    {
                        for (int x = chunkLoopArea.Pos.X; x <= chunkLoopArea.Max.X; x++)
                        {
                            Chunk chunk = ChunkSystem.GetOrCreate(x, y);

                            ChunkLighting lighting = chunk.GetComponent<ChunkLighting>();

                            // We want to stay inside the chunk,
                            // so each of the nearby chunks are enumerated over
                            // and the bounds are calculated based on the screen coordinates
                            lighting.UpdateArea(new Rect32(
                                // This represents the top left
                                Math.Max(tileLoopArea.X, chunk.TileRectangle.X),
                                Math.Max(tileLoopArea.Y, chunk.TileRectangle.Y),
                                // This is the bottom right
                                Math.Min(tileLoopArea.Max.X, chunk.TileRectangle.Max.X - 1),
                                Math.Min(tileLoopArea.Max.Y, chunk.TileRectangle.Max.Y - 1)
                            ));

                            lighting.ApplyColors();
                        }
                    }
                }

                orig(firstX, lastX, firstY, lastY);
            };
        }

        public override void OnInit()
        {
            int textureWidth = Chunk.TileRectangle.Width;
            int textureHeight = Chunk.TileRectangle.Height;

            Colors = new Surface<Color>(textureWidth, textureHeight);
            Texture = new RenderTarget2D(Main.graphics.GraphicsDevice, textureWidth, textureHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }
        // TODO: Eventually swap over to OnLose or something, requires ModSystem from 1.4
        public override void Unload()
        {
            if (Texture != null)
            {
                Texture.Dispose();

                Texture = null;
            }
        }

        public void UpdateArea(Rect32 rect)
        {
            for (int y = rect.Pos.Y; y <= rect.Max.Y; y++)
            {
                for (int x = rect.Pos.X; x <= rect.Max.X; x++)
                {
                    Colors[x - Chunk.TileRectangle.X, y - Chunk.TileRectangle.Y] = Lighting.GetColor(x, y);
                }
            }
        }
        public void ApplyColors()
        {
            //lock (Texture)
            {
                Texture.SetData(Colors.Data);
            }
        }
    }
}
*/