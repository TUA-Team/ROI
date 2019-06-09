using Microsoft.Xna.Framework.Graphics;
using ROI.GUI.VoidUI;

namespace ROI.Manager
{
	internal class UIManager : BaseInstanceManager<UIManager>
	{
		public override void Initialize()
		{
            VoidPillarHealthBar.Load();
		}

        public override void Unload()
        {
            VoidPillarHealthBar.Unload();
        }

        public void PostDrawInterface(SpriteBatch spriteBatch)
        {
            VoidPillarHealthBar.FindPillar();
            VoidPillarHealthBar.Draw(spriteBatch);
        }
    }
}
