using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ROI.Models
{
    /*
	public interface ITreeHook
	{
		void PostDrawTreeTop(SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle, Vector2 origin);
	}
	*/
    public interface ICamerable
    {
        bool CurrentlyExecuting { get; set; }
        bool ScrollingEffect { get; }

        Vector2 CameraPosition { get; }
    }

    public interface IMobCamerable<T> where T : ModNPC, ICamerable
    {
        bool AnimationAI();
    }
}