namespace ROI.Commons.Users
{
    public sealed class Developer : User
    {
        public Developer(long steamId64, string displayName, ulong discordId) : base(steamId64, displayName, discordId)
        {
        }
    }
}