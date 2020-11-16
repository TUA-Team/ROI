using System.Collections.Generic;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.World.Generation;

namespace ROI.Content.Subworlds.Wasteland
{
    public class WastelandDepthSubworld : Subworld
    {
        public override int width => Main.maxTilesX;
        public override int height => 1000;
        public override bool saveModData => true;

        //public override UIState loadingUIState => new UIState();

        public override List<GenPass> tasks => new List<GenPass>()
            {
                new PassLegacy("ROI: WastelandGeneration", delegate (GenerationProgress progress)
                {
                    WastelandWorldBuilding.WastelandGeneration(progress);
                })
            };
    }
}