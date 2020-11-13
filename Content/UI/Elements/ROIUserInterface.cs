using Terraria.UI;

namespace ROI.Content.UI.Elements
{
    public sealed class ROIUserInterface<TState> : UserInterface where TState : ModUIState
    {
        public new TState CurrentState => base.CurrentState as TState;
    }
}
