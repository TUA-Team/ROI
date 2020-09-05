using Terraria.UI;

namespace ROI.Models
{
    public sealed class ROIUserInterface<TState> : UserInterface where TState : UIState
    {
        public new TState CurrentState => base.CurrentState as TState;
    }
}
