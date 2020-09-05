using ROI.Backgrounds;
using ROI.Loaders;

namespace ROI
{
    public sealed partial class ROIMod
    {
        private void LoadClient() {
            UserLoader.Initialize(this);

            BackgroundLoader.Load();
        }


        private void UnloadClient() {
            // No point in uninitializing the UserHelper since its so small. Might add it later.
            BackgroundLoader.Unload();
        }
    }
}
