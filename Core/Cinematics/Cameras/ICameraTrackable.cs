using Microsoft.Xna.Framework;

namespace ROI.Core.Cinematics.Cameras
{
    public interface ICameraTrackable
    {
        bool CurrentlyExecuting { get; set; }
        bool ScrollingEffect { get; }

        Vector2 CameraPosition { get; }
    }
}