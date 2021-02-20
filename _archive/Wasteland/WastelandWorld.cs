using LiquidAPI;
using LiquidAPI.LiquidMod;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland
{
    internal sealed class WastelandWorld : ModWorld
    {
        public static int activeHotW;


        public override void Initialize()
        {
            activeHotW = -1;
        }


        public override void PostDrawTiles()
        {
            ModLiquid waste = LiquidRegistry.GetLiquid(mod, nameof(Liquids.PlutonicWaste));
            ModLiquid lava = LiquidRegistry.GetLiquid(LiquidAPI.LiquidAPI.Instance, nameof(LiquidAPI.Vanilla.Lava));
            for (int i = (int)Main.screenPosition.X / 16 - 16; i < (Main.screenPosition.X + Main.graphics.PreferredBackBufferWidth) / 16 + 16; i++)
            {
                for (int j = (int)Main.screenPosition.Y / 16 - 16; j < (Main.screenPosition.Y + Main.graphics.PreferredBackBufferWidth) / 16 + 16; j++)
                {
                    if (!WorldGen.InWorld(i, j) || !WorldGen.InWorld(i, j - 1))
                        break;

                    LiquidRef reference = LiquidWorld.grid[i, j];
                    LiquidRef referenceUp = LiquidWorld.grid[i, j - 1];
                    LiquidRef referenceUpRight = LiquidWorld.grid[i + 1, j - 1];

                    // TODO: this will be eventually removed when LiquidAPI is refactored
                    if (reference.LiquidType == null)
                        continue;

                    if (reference.LiquidType.Type == waste.Type && !referenceUp.HasLiquid && reference.HasLiquid)
                    {
                        float preBrightness = Lighting.brightness;
                        Lighting.brightness = 0.2f;
                        Lighting.AddLight(i, j, 0.01f, 0.5f, 0.01f);
                        Lighting.brightness = preBrightness;
                        // TODO: Move this into a proper method for server
                        if (Main.netMode != NetmodeID.Server && Main.hasFocus && Main.rand.Next(5000) == 0)
                        {
                            if (reference.Amount > 200 && !referenceUpRight.Tile.active())
                            {
                                //int properOffset = j * 16 + 16 - (16 - reference.Amount / 16);
                                Projectile.NewProjectile(new Vector2(i * 16, (j - 1) * 16 + 8f),
                                    Vector2.Zero,
                                    ModContent.ProjectileType<Projectiles.WastelandSurfaceBubble>(),
                                    0,
                                    0);
                            }

                            // TODO: is this check necessary?
                            if (Subworld.IsActive<WastelandDepthSubworld>())
                            {
                                int bubbleType = Main.rand.Next(0, 3);

                                int projectile = Projectile.NewProjectile(new Vector2(i * 16, (j + 2) * 16),
                                    new Vector2(Main.rand.NextFloat(-0.36f, 0.36f), -0.5f),
                                    ModContent.ProjectileType<Projectiles.WastelandBubble>(),
                                    0,
                                    0,
                                    255,
                                    bubbleType);
                                Main.projectile[projectile].ai[0] = bubbleType;
                            }
                        }
                    }

                    else if (reference.LiquidType.Type == lava.Type)
                    {
                        float preBrightness = Lighting.brightness;
                        Lighting.brightness = 0.7f;
                        Lighting.AddLight(i, j, 1.00f, 0.25f, 0.10f);
                        Lighting.brightness = preBrightness;
                    }
                }
            }
        }
    }
}
