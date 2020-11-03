using System.Collections.Generic;
using LiquidAPI.LiquidMod;
using LiquidAPI.Vanilla;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.World.Generation;

namespace ROI.Worlds.Subworlds.Wasteland
{
    public class TheWastelandDepthSubworld : Subworld
    {
        public override int width => Main.maxTilesX;
        public override int height => 1000;
        public override bool saveModData => true;
        public override bool disablePlayerSaving => false;
        public override bool saveSubworld => false;

        public override bool noWorldUpdate => false;

        public override UIState loadingUIState => new UIState();

        public override ModWorld modWorld => null;

        public override List<GenPass> tasks => GetPassList();


        public List<GenPass> GetPassList()
        {
            return new List<GenPass>()
            {
                new PassLegacy("WastelandGeneration", delegate(GenerationProgress progress)
                {
                    WastelandWorldGeneration.WastelandGeneration(progress);
                }),
                new PassLegacy("Settle Liquid", delegate(GenerationProgress progress)
                {
                    return;
                    WorldGen.waterLine = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2;
                    WorldGen.waterLine += WorldGen.genRand.Next(-100, 20);
                    WorldGen.lavaLine = WorldGen.waterLine + WorldGen.genRand.Next(50, 80);

                    //Liquid.QuickWater(2);
                    //WorldGen.WaterCheck();
                    int num4 = 0;
                    Liquid.quickSettle = true;
                    int num5 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                    float num6 = 0f;
                    while (Liquid.numLiquid > 0 && num4 < 100000)
                    {
                        num4++;
                        float num7 = (float)(num5 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float)num5;
                        if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num5)
                            num5 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;

                        if (num7 > num6)
                            num6 = num7;
                        else
                            num7 = num6;

                        progress.Value = (num7 * 100f / 2f + 50f);
                        Liquid.UpdateLiquid();
                    }

                    Liquid.quickSettle = false;
                    //WorldGen.WaterCheck();
                })
            };
        }

        
    }
}
