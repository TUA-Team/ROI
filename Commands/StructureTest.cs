using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.Commands
{
    public class StructureTest : ModCommand
    {
        public override bool Autoload(ref string name) => ROIMod.DEBUG;

        public override string Command => "structuretest";

        public override CommandType Type => CommandType.World;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            switch (args[0])
            {
                case "perlinmass":
                    var noise = new FastNoiseLite();
                    noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
                    noise.SetFractalOctaves(5);
                    noise.SetFrequency(0.05f);

                    var pos = caller.Player.position.ToTileCoordinates16() - new Point16(50, 60);
                    for (int x = pos.X; x < pos.X + 100; x++)
                    {
                        for (int y = pos.Y; y < pos.Y + 50; y++)
                        {
                            var rand = noise.GetNoise(x, y);
                            if (rand < -0.1f)
                            {
                                WorldGen.PlaceTile(x, y, ModContent.TileType<WastelandRock>());
                                //WorldGen.TileRunner(pos.X, i, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(50, 60), ModContent.TileType<WastelandRock>());
                            }
                            else
                            {
                                WorldGen.KillTile(x, y, noItem: true);
                            }
                        }
                    }
                    break;
            }
        }
    }
}
