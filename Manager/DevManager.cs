using System.Reflection;
using Terraria.ModLoader;

namespace ROI.Manager
{
    internal sealed class DevManager : BaseInstanceManager<DevManager>
    {
        public string curSteam;

        private static readonly string[] devIDs = new string[]
        {
            "76561198062217769", // Dradonhunter11
            "76561197970658570", // 2grufs
            "76561193945835208", // DarkPuppey
            "76561193830996047", // Gator
            "76561198098585379", // Chinzilla00
            "76561198265178242", // Demi
            "76561193989806658", // SDF
            "76561198193865502", // Agrair
            "76561198108364775", // HumanGamer
            "76561198046878487", // Webmilio
            "76561198008064465", // Rartrin
            "76561198843721841", // Skeletony
        };

        public bool CheckDev()
        {
            curSteam = (string)(typeof(ModLoader).GetProperty("SteamID64",
                BindingFlags.Static | BindingFlags.NonPublic).GetAccessors(true)[0]
                .Invoke(null, new object[] { }));
            for (int i = 0; i < 12; i++)
            {
                if (devIDs[i] == curSteam) return true;
            }
            return false;
        }

        public override void Initialize()
        {
            
        }
    }
}
