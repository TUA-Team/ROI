using ROI.Backgrounds;
using ROI.Backgrounds.Underworld;
using ROI.Helpers;

namespace ROI
{
    public sealed partial class ROIMod
    {
        private void LoadClient()
        {
            UserHelper.Initialize(this);

            BackgroundLoader.Load();
        }


        private void UnloadClient()
        {
            // No point in uninitializing the UserHelper since its so small. Might add it later.
            BackgroundLoader.Unload();
        }
    }
}
