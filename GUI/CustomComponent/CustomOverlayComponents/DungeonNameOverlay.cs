using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityCore.API.Overlay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Worlds.Subworlds;
using SubworldLibrary;
using Terraria;

namespace ROI.GUI.CustomComponent.CustomOverlayComponents
{
    class DungeonNameOverlay : OverlayComponent
    {
        public override void Update(GameTime time)
        {
            if (!Subworld.IsActive<VoidRiftSubworld>()) return;
            Vector2 textSize = Main.fontDeathText.MeasureString(VoidRiftModWorld.currentFloorInfo?.floorName);
            Vector2 newPosition = new Vector2(Main.screenWidth / 2 - textSize.X / 2, Main.screenWidth / 2 - (Main.screenWidth / 15));
            SetPosition(newPosition);
            SetDimension(textSize);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!Subworld.IsActive<VoidRiftSubworld>()) return;
            Rectangle bound = GetBound();
            Utils.DrawBorderStringFourWay(sb, Main.fontDeathText, VoidRiftModWorld.currentFloorInfo?.floorName, bound.TopLeft().X, bound.TopLeft().Y,  Color.White * 0.5f, Color.Gray, Vector2.Zero, 1f);
            //ChatManager.DrawColorCodedString(sb, Main.fontDeathText, $"{span.Hours}:{span.Minutes % 60}:{span.Seconds % 60}", bound.TopLeft(), Color.White, 0f, Vector2.Zero, Vector2.One);
        }
    }
}
