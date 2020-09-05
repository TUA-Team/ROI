namespace ROI.Models.Users
{
    public class User
    {
        public User(long steamId64, string displayName, ulong discordId)
        {
            SteamId64 = steamId64;
            DisplayName = displayName;
            DiscordId = discordId;
        }

        public static implicit operator long(User user) => user.SteamId64;
        public static implicit operator string(User user) => user.DisplayName;

        public static implicit operator bool(User user) => user.IsCurrentUser;

        public long SteamId64 { get; }
        public string DisplayName { get; }
        public ulong DiscordId { get; }

        public bool IsCurrentUser { get; internal set; }
    }
}