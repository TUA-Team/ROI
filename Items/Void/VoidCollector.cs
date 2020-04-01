using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.Items.Void
{
    internal class VoidCollector : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVoidCollector());
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
        }

        public override void UpdateInventory(Player player)
        {
            var plr = player.GetModPlayer<ROIPlayer>();
            plr.voidCollector = true;
        }
    }

    internal class DrawAnimationVoidCollector : DrawAnimation
    {
        int increment;

        public DrawAnimationVoidCollector()
        {
            Frame = 0;
            FrameCounter = 0;
            FrameCount = 12;
            TicksPerFrame = 5;
            increment = 1;
        }

        public override void Update()
        {
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<VoidFragment>()))
            {
                if (++FrameCounter == 5)
                {
                    FrameCounter = 0;
                    Frame += increment;
                    if (Frame == 11)
                    {
                        increment = -1;
                    }
                    else if (increment == -1 && Frame == 8)
                    {
                        increment = 1;
                    }
                }
            }
            else if (Frame != 0)
            {
                if (++FrameCounter == 5)
                {
                    FrameCounter = 0;
                    Frame += increment;
                    if (Frame == 8)
                    {
                        Frame = 0;
                        increment = 1;
                    }
                    else if (Frame == 11)
                    {
                        Frame = 10;
                        increment = -1;
                    }
                }
            }
        }

        public override Rectangle GetFrame(Texture2D texture)
        {
            return texture.Frame(1, FrameCount, 0, Frame);
        }
    }
}
