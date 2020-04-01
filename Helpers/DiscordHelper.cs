using DiscordRPC;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Helpers
{
    //TODO: set this up properly
    public static class DiscordHelper
    {
        private static RichPresence _presence = new RichPresence();

        private static DiscordRpcClient _client;

        private static string _currentState;

        public static void RegisterRPC(Mod initializer)
        {
            _currentState = Main.netMode == 0
                ? Main.rand.Next(new string[] { "Playing Alone", "Lone Samurai", "Singleplayer" })
                : Main.rand.Next(new string[] { "Playing With Friends", "Multiplayer" });

            _presence = new RichPresence()
            {
                Details = Environment.Is64BitProcess ? "In Main Menu (64x)" : "In Main Menu",
                State = _currentState,
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = "Loading mods"
                }
            };
            if (Main.netMode != 0)
            {
                _presence.Party = new Party()
                {
                    Size = Main.ActivePlayersCount
                };
            }
            _client = new DiscordRpcClient("528086919670792233");
            _client.OnError += (sender, args) =>
            {
                initializer.Logger.ErrorFormat("Rich Presence failed. Code {1}, {0}", args.Message, args.Code);
            };
            _presence.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            _client.Initialize();
            _client.SetPresence(_presence);
            _client.Invoke();
        }
    }
}
