using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.Items.Void
{
    public class VoidCollector : ROIItem
    {
        public VoidCollector() : base("Void Collector", "A mysterious orb used to collect void", 32, 32) { }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(item.type, new DrawAnimationVoidCollector());
        }

        public override void UpdateInventory(Player player)
        {
            var plr = ROIPlayer.Get(player);
            plr.voidCollector = true;
        }

        private class DrawAnimationVoidCollector : DrawAnimation
        {
            int increment;

            public DrawAnimationVoidCollector()
            {
                Frame = 0;
                FrameCounter = 0;
                FrameCount = 15;
                TicksPerFrame = 6;
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
                            increment = 1;
                        }
                        else if (Frame == 14)
                        {
                            Frame = 0;
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
}
