using LiquidAPI;
using LiquidAPI.LiquidMod;
using ROI.Content.Buffs.Void;
using ROI.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Worlds
{
    public sealed partial class ROIWorld : ModWorld
    {
        public override void PreUpdate()
        {
            if (StrangePresenceDebuff)
            {
                for (int i = 0; i < Main.player.Length; i++)
                    Main.player[i].AddBuff<PillarPresence>(1, true);
            }
        }

        public override void PostDrawTiles()
        {
/*            ModLiquid waste = LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "PlutonicWaste");
            for (int i = (int)(Main.screenPosition.X - 3) / 16; i < (Main.screenPosition.X - 3 + Main.graphics.PreferredBackBufferWidth + 3) / 16; i++)
            {
                for (int j = (int)(Main.screenPosition.Y - 3) / 16; j < (Main.screenPosition.Y - 3 + Main.graphics.PreferredBackBufferWidth + 3) / 16; j++)
                {
                    if (!WorldGen.InWorld(i, j) || !WorldGen.InWorld(i, j - 1))
                        break;

                    LiquidRef reference = LiquidWorld.grid[i, j];
                    LiquidRef referenceUp = LiquidWorld.grid[i, j - 1];
                    if (reference.Type.Type == waste.Type && !referenceUp.Tile.active() && !referenceUp.HasLiquid)
                    {
                        Lighting.AddLight(i, j, 0.01f, 1f * 0.8f, 0.01f);
                    }
                }
            }*/
        }


        public bool StrangePresenceDebuff { get; internal set; }

        public int PillarSpawningTime { get; private set; }
    }
}