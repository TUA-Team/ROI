using ROI.Commons.Users;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace ROI.Helpers
{
    public sealed class UserLoader : BaseLoader
    {
        private readonly bool _initialized = false;

        private readonly List<Developer> _activeDevelopers = new List<Developer>();


        public override void Initialize(Mod mod)
        {
            if (_initialized) return;

            try
            {
                string unparsedSteamID64 = typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null).ToString();

                if (!string.IsNullOrWhiteSpace(unparsedSteamID64))
                {
                    HasSteamId64 = true;
                    SteamId64 = long.Parse(unparsedSteamID64);
                }
            }
            catch (Exception)
            {
                mod.Logger.Info("Unable to fetch SteamID, assuming no Steam is present.");
            }

            if (!HasSteamId64) return;

            foreach (Developer developer in _activeDevelopers)
                if (developer.SteamId64 == SteamId64)
                {
                    CurrentUser = developer;

                    developer.IsCurrentUser = true;

                    CurrentDeveloper = developer;
                    IsDeveloper = true;

                    break;
                }
        }


        public bool HasSteamId64 { get; private set; }
        public long SteamId64 { get; private set; }


        public User CurrentUser { get; private set; }

        public Developer CurrentDeveloper { get; private set; }
        public bool IsDeveloper { get; private set; }
    }
}
