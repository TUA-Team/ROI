using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ROI
{
    public static class TextureCache
    {
        public static Dictionary<string, Texture2D> TorchFlameTexture;
        public static Dictionary<string, Texture2D> ChandelierFlameTexture;

        public static void Initialize()
        {
            TorchFlameTexture = LoadTorchFlame();
            ChandelierFlameTexture = LoadChandelierFlame();
        }

        public static void Unload()
        {
            TorchFlameTexture.Clear();
            ChandelierFlameTexture.Clear();
            TorchFlameTexture = null;
            ChandelierFlameTexture = null;
        }

        public static Dictionary<string, Texture2D> LoadTorchFlame()
        {
            return new Dictionary<string, Texture2D>()
            {
                ["Wastebrick_Torch"] = ROIMod.instance.GetTexture("Tiles/Furniture/Wastebrick_Torch_Flame")
            };
        }

        public static Dictionary<string, Texture2D> LoadChandelierFlame()
        {
            return new Dictionary<string, Texture2D>()
            {
                ["Wastebrick_Chandelier"] = ROIMod.instance.GetTexture("Tiles/Furniture/Wastebrick_Chandelier_Flame")
            };
        }
    }
}
