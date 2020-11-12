using ROIWorld = ROI.Worlds.ROIWorld;
#if DEBUG
using Microsoft.Xna.Framework;
using ROI.Worlds.Structures.Wasteland;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Commands
{
    class GenerateStructure : ModCommand
    {
        public override bool Autoload(ref string name) => ROIMod.debug;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Vector2 position = caller.Player.position;
            Point16 positionInWorld = position.ToPoint16();
            switch (args[0].ToLower())
            {
                case "wastelandgrotto":
                    Wasteland_Grotto.Generate(positionInWorld.X, positionInWorld.Y);
                    break;
                case "wastelake":
                    Wasteland_Lake.Generate(position);
                    break;
            }
        }

        public override string Command => "genStruct";
        public override CommandType Type => CommandType.World;
    }
}
#endif
