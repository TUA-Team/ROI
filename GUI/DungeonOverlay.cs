using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityCore.API.Overlay;
using Microsoft.Xna.Framework.Input;
using ROI.GUI.CustomComponent.CustomOverlayComponents;
using ROI.Worlds.Subworlds;

namespace ROI.GUI
{
    class DungeonOverlay : ModScreenOverlay
    {
        public override bool active => VoidRiftModWorld.InVoidRift && Keyboard.GetState().IsKeyDown(Keys.Tab);
        

        public DungeonOverlay()
        {
            AddComponent(new TimeOverlay());
            AddComponent(new DungeonNameOverlay());
        }

        
    }
}
