using System.Collections.Generic;
using ROI.Content.Worlds;
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
        public override bool disablePlayerSaving => false;
        public override bool saveSubworld => false;

        public override UIState loadingUIState => new UIState();

        public override ModWorld modWorld => null;

        public override List<GenPass> tasks => GetPassList();


        public List<GenPass> GetPassList()
        {
            return new List<GenPass>()
            {
                new PassLegacy("WastelandGeneration", delegate(GenerationProgress progress)
                {
                    ROIWorld.WastelandGeneration(progress);
                })
            };
        }


    }
}