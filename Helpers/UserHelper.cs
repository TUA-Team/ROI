using System;
using System.Collections.Generic;
using System.Reflection;
using ROI.Commons.Users;
using Terraria.ModLoader;

namespace ROI.Helpers
{
    public static class UserHelper
    {
        private static bool _initialized = false;

        private static readonly List<Developer> _activeDevelopers = new List<Developer>();


        public static void Initialize(Mod initializer)
        {
            if (_initialized) return;

            Initializer = initializer;

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
                Initializer.Logger.Info("Unable to fetch SteamID, assuming no steam is present.");
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


        public static bool HasSteamId64 { get; private set; }
        public static long SteamId64 { get; private set; }


        public static User CurrentUser { get; private set; }

        public static Developer CurrentDeveloper { get; private set; }
        public static bool IsDeveloper { get; private set; }


        public static Mod Initializer { get; private set; }
    }
}
