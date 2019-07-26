using ROI.Helpers;

namespace ROI
{
    public sealed partial class ROIMod
    {
        private void LoadClient()
        {
            UserHelper.Initialize(this);
        }


        private void UnloadClient()
        {
            // No point in uninitializing the UserHelper since its so small. Might add it later.
        }
    }
}
