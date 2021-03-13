using ROI.Commons;
using Terraria.ModLoader;

namespace ROI.Core.Cinematics.Cameras
{
    public interface IMobCamerable<T> where T : ModNPC, ICameraTrackable
    {
        bool AnimationAI();
    }
}
