using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityCore.API.Overlay;
using ROI.GUI.CustomComponent.CustomOverlayComponents;
using ROI.Worlds.Subworlds;

namespace ROI.GUI
{
    class DungeonOverlay : ModScreenOverlay
    {
        public override bool active => VoidRiftModWorld.InVoidRift;

        public DungeonOverlay()
        {
            AddComponent(new TimeOverlay());
        }

        
    }
}
