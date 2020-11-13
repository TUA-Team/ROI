using Terraria.ModLoader;
using Terraria.UI;

namespace ROI.Content.UI.Elements
{
    public abstract class ModUIState : UIState
    {
        protected readonly Mod _mod;

        public ModUIState(Mod mod) => _mod = mod;
    }
}
