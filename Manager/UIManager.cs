using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Collections.Generic;
using Terraria;

namespace ROI.Manager
{
    internal class UIManager : BaseInstanceManager<UIManager>
	{
        private Queue<string> loreQueue;
        private ushort loreUIProgress;
        private bool loreOpen;
        private bool loreClose;
        private bool loreAppear;
        private bool loreDisappear;
        private bool loreTextAppear;
        private string loreCurrent;
        // actually half the banner's size
        private int loreBannerSize;
        private Texture2D loreEnds;
        private Texture2D loreBG;

        public override void Initialize()
		{
            loreQueue = new Queue<string>();
            loreUIProgress = 0;
            loreEnds = mod.GetTexture("Textures/" + nameof(loreEnds));
            loreBG = mod.GetTexture("Textures/" + nameof(loreBG));
		}

        protected override void UnloadInternal()
        {
            loreEnds?.Dispose();
        }

        public void QueueLore(string name)
        {
            if (loreQueue.Count == 0) loreCurrent = name;
            else loreQueue.Enqueue(name);
            loreAppear = true;
            loreDisappear = false;
            loreOpen = false;
            loreClose = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            while (loreCurrent != null)
            {
                loreBannerSize = ((int)Main.fontItemStack.MeasureString(loreCurrent).X + 30) >> 1;
                loreUIProgress++;

                // coming down
                if (loreAppear && loreUIProgress <= 15 + loreEnds.Height)
                {
                    spriteBatch.Draw(loreEnds, new Vector2(Main.screenWidth * .5f,
                        loreUIProgress - loreEnds.Height), Color.White);
                    spriteBatch.Draw(loreEnds, new Vector2(Main.screenWidth * .5f, 
                        loreUIProgress - loreEnds.Height), null, Color.White, 0, loreEnds.Size() * .5f,
                        0, SpriteEffects.FlipHorizontally, 0);
                    if (loreUIProgress == 15 + loreEnds.Height)
                    {
                        loreUIProgress = 0;
                        loreAppear = false;
                        loreOpen = true;
                    }
                }

                // opening
                else if (loreOpen && loreUIProgress <= loreBannerSize)
                {
                    spriteBatch.Draw(loreBG, new Rectangle((int)(Main.screenWidth * .5f) - loreUIProgress,
                        loreEnds.Height - loreBG.Height + 15, loreUIProgress * 2, loreBG.Height), Color.White);
                    spriteBatch.Draw(loreEnds, new Vector2(Main.screenWidth * .5f + loreUIProgress, 15), Color.White);
                    spriteBatch.Draw(loreEnds, new Vector2(Main.screenWidth * .5f - loreUIProgress, 15), null, 
                        Color.White, 0, loreEnds.Size() * .5f, 0, SpriteEffects.FlipHorizontally, 0);
                    if (loreUIProgress == loreBannerSize)
                    {
                        loreUIProgress = 0;
                        loreOpen = false;
                        loreTextAppear = true;
                    }
                }

                // revealing of text
                else if (loreTextAppear && loreUIProgress <= 120)
                {
                    spriteBatch.Draw(loreBG, new Rectangle((int)(Main.screenWidth * .5f) - loreBannerSize,
                        loreEnds.Height - loreBG.Height + 15, loreBannerSize * 2, loreBG.Height), Color.White);
                    spriteBatch.Draw(loreEnds, new Vector2(Main.screenWidth * .5f + loreBannerSize, 15), Color.White);
                    spriteBatch.Draw(loreEnds, new Vector2(Main.screenWidth * .5f - loreBannerSize, 15), null,
                        Color.White, 0, loreEnds.Size() * .5f, 0, SpriteEffects.FlipHorizontally, 0);
                    // TODO: draw strings based on loreUIProgress
                }
            }
        }
    }
}
