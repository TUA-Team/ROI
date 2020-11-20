using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using ROI.Worlds;
using ROI.Content.Subworlds.Wasteland.WorldBuilding.Tiles;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding
{
    internal sealed class WastelandLostCave : WastelandCave
    {
        public WastelandLostCave(Rectangle rectangle) : base(rectangle)
        {
        }

        public override string caveTypeName => "Lost_Wood";

        //Insert shader effect here but without being a real shader so Idk
        public override void ClientSideVisualEffect(Player player)
        {
            base.ClientSideVisualEffect(player);
        }

        public override void Generate(GenerationProgress progress)
        {
            Rectangle bound = CaveBound;

            WorldGen.TileRunner(CaveBound.Center.X, CaveBound.Center.Y, WorldGen.genRand.Next(70, 100), 30, -1, true, WorldGen.genRand.Next(7, 10), WorldGen.genRand.Next(-2, 2));
            WorldGen.TileRunner(CaveBound.Center.X, CaveBound.Center.Y, WorldGen.genRand.Next(70, 100), 30, -1, true, WorldGen.genRand.Next(-10, -7), WorldGen.genRand.Next(-2, 2));

            for (int i = bound.Y; i < bound.Y + height; i++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                {
                    GeneralWorldHelper.TileRunner(bound.X, i, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(50, 60), ModContent.TileType<WastelandRock>(), 0, true, 1f);
                }
            }

            for (int i = bound.Y; i < bound.Y + height; i++)
            {
                if (WorldGen.genRand.Next(20) == 0)
                {
                    GeneralWorldHelper.TileRunner(bound.X + width, i, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(50, 60), ModContent.TileType<WastelandRock>(), 0, true, -1f);
                }
            }

            GeneralWorldHelper.TileRunner(bound.Center.X - 16, bound.Center.Y + 4, 10, 20, ModContent.TileType<WastelandDirt>(), 0, true, 1f, 0f, true);
            RefiningTileRunnerAftermath();
            SpreadGrass();
            AddLeave();
        }

        private void GenerateHut()
        {
            Point startLocation = CaveBound.Center;
            int startScanLocation = CaveBound.Y - 10;

        }

        private void SpreadGrass()
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (Main.tile[i, j].type == ModContent.TileType<WastelandDirt>() && DetectHowManySideAreTouchingOtherBlock(i, j) <= 3)
                    {
                        WorldGen.KillTile(i, j, false, false, true);
                        WorldGen.PlaceTile(i, j, ModContent.TileType<WastelandGrass>(), false, false, -1, 0);
                    }
                }
            }
        }

        private void FloodBottom()
        {

        }

        private void RefiningTileRunnerAftermath()
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (!IsTileTouchingDirectly(i, j))
                    {
                        Main.tile[i, j].active(false);
                    }
                }
            }
        }

        private void AddLeave()
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (!IsTileUnderActive(i, j))
                    {
                        if (Main.tile[i, j].type == ModContent.TileType<WastelandRock>())
                        {
                            Main.tile[i, j].type = TileID.GreenMoss;
                        }

                        if (Main.tile[i, j - 1].type == ModContent.TileType<WastelandRock>() ||
                            Main.tile[i, j - 1].type == ModContent.TileType<WastelandGrass>() ||
                            Main.tile[i, j - 1].type == ModContent.TileType<WastelandDirt>())
                        {
                            int leaveHeight = WorldGen.genRand.Next(3, 10);
                            for (int k = 0; k < leaveHeight; k++)
                            {
                                if (!Main.tile[i, j + k + 1].active())
                                {
                                    WorldGen.PlaceWall(i, j + k, WallID.LivingLeaf);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsTileTouchingDirectly(int x, int y)
        {
            return Main.tile[x + 1, y].active() || Main.tile[x - 1, y].active() || Main.tile[x, y + 1].active() || Main.tile[x, y - 1].active();
        }

        private bool IsTileUnderActive(int x, int y)
        {
            return Main.tile[x, y + 1].active();
        }

        private int DetectHowManySideAreTouchingOtherBlock(int i, int j)
        {
            int side = 0;
            if (Main.tile[i + 1, j].active())
            {
                side += 1;
            }
            if (Main.tile[i - 1, j].active())
            {
                side += 1;
            }
            if (Main.tile[i, j + 1].active())
            {
                side += 1;
            }
            if (Main.tile[i, j - 1].active())
            {
                side += 1;
            }

            return side;
        }
    }
}