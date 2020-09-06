using ROI.Models.Backgrounds;
using ROI.Models.Backgrounds.Underworld;
using Terraria.ModLoader;

namespace ROI.Loaders
{
    public class BackgroundLoader : CollectionLoader<Background>
    {
        public override void Initialize(Mod mod) {
            Wasteland = Add(new WastelandBackground(mod)) as WastelandBackground;
        }


        public WastelandBackground Wasteland { get; private set; }
    }
}