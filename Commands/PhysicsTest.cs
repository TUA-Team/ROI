using Microsoft.Xna.Framework;
using ROI.Content.Biomes.Wasteland;
using Terraria.ModLoader;

namespace ROI.Commands
{
    public class PhysicsTest : ModCommand
    {
        public override bool Autoload(ref string name) => ROIMod.DEBUG;

        public override string Command => "physicstest";

        public override CommandType Type => CommandType.World;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            switch (args[0])
            {
                case "wastelandvine":
                    WastelandWorld.vineContext.AddVine(caller.Player.position - new Vector2(0, 60), 4);
                    break;
            }
        }
    }
}
