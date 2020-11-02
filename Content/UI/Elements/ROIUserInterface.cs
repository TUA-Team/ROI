using Terraria.UI;

namespace ROI.Content.UI.Elements
{
    public sealed class ROIUserInterface<TState> : UserInterface where TState : ROIState
    {
        public new TState CurrentState => base.CurrentState as TState;
    }
}
