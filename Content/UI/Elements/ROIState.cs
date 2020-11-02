using Terraria.UI;

namespace ROI.Content.UI.Elements
{
    public abstract class ROIState : UIState
    {
        protected readonly ROIMod _mod;

        public ROIState(ROIMod mod) => _mod = mod;
    }
}
