using Newtonsoft.Json;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.API
{
    // TODO: Make this work with tile entities, somehow
    public class StructureTemplate
    {
        private readonly Tile[,] tiles;

        public StructureTemplate(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                tiles = JsonConvert.DeserializeObject<Tile[,]>(reader.ReadToEnd());
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        if (tiles[i, j].wall >= WallLoader.WallCount)
                        {
                            tiles[i, j].wall = 0;
                        }
                        if (tiles[i, j].type >= TileLoader.TileCount)
                        {
                            tiles[i, j].type = 0;
                        }
                    }
                }
            }
        }

        public void Paste(Point16 origin)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (WorldGen.InWorld(origin.X + x, origin.Y + y))
                    {
                        Tile target = Framing.GetTileSafely(origin.X + x, origin.Y + y);
                        int cycledX = (x % width + width) % width;
                        int cycledY = (y % height + height) % height;
                        if (tiles[cycledX, cycledY].active())
                        {
                            target.CopyFrom(tiles[cycledX, cycledY]);
                        }
                    }
                }
            }
            // TODO: Experiment with WorldGen.stopDrops = true;
            // TODO: Button to ignore TileFrame?
            for (int i = origin.X; i < origin.X + width; i++)
            {
                for (int j = 0; j < origin.Y + height; j++)
                {
                    WorldGen.SquareTileFrame(i, j, false); // Need to do this after stamp so neighbors are correct.
                    if (Main.netMode == NetmodeID.MultiplayerClient && Framing.GetTileSafely(i, j).liquid > 0)
                    {
                        NetMessage.sendWater(i, j); // Does it matter that this is before sendtilesquare?
                    }
                }
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(-1, origin.X + width / 2, origin.Y + height / 2, Math.Max(width, height));
            }
        }
    }
}
