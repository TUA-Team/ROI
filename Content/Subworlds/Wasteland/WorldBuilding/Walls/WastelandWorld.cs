using LiquidAPI;
using LiquidAPI.LiquidMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding.Walls
{
    internal class WastelandWorld : ModWorld
    {
        public override void PostDrawTiles()
        {
            ModLiquid waste = LiquidRegistry.GetLiquid(mod, nameof(Liquids.PlutonicWaste));
            ModLiquid lava = LiquidRegistry.GetLiquid(LiquidAPI.LiquidAPI.Instance, "Lava");
            for (int i = (int)(Main.screenPosition.X) / 16 - 16; i < (Main.screenPosition.X + Main.graphics.PreferredBackBufferWidth) / 16 + 16; i++)
            {
                for (int j = (int)(Main.screenPosition.Y) / 16 - 16; j < (Main.screenPosition.Y + Main.graphics.PreferredBackBufferWidth) / 16 + 16; j++)
                {
                    if (!WorldGen.InWorld(i, j) || !WorldGen.InWorld(i, j - 1))
                        break;

                    LiquidRef reference = LiquidWorld.grid[i, j];
                    LiquidRef referenceUp = LiquidWorld.grid[i, j - 1];
                    if (reference.LiquidType.Type == waste.Type && !referenceUp.HasLiquid && reference.HasLiquid)
                    {
                        float preBrightness = Lighting.brightness;
                        Lighting.brightness = 0.2f;
                        Lighting.AddLight(i, j, 0.01f, 0.5f, 0.01f);
                        Lighting.brightness = preBrightness;
                        if (Main.netMode != NetmodeID.Server && Main.rand.Next(5000) == 0)
                        {
                            int properOffset = (j * 16) + 16 - (16 - reference.Amount / 16);
                            Projectile.NewProjectile(new Vector2(i * 16, properOffset + 2), Vector2.Zero, ModContent.ProjectileType<Projectiles.WastelandSurfaceBubble>(), 0, 0);
                        }
                    }
                    if (reference.LiquidType.Type == lava.Type)
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
