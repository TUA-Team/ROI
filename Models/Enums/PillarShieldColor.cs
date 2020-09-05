namespace ROI.Models.Enums
{
    internal enum PillarShieldColor : byte
    {
        /// <summary>
        /// Red color shield, will drain health from the player directly
        /// </summary>
        Red = 0,
        /// <summary>
        /// Purple color shield, weaken the player by 75%
        /// </summary>
        Purple = 1,
        /// <summary>
        /// Black color shield, blind the player, making the fight harder, permanent night
        /// </summary>
        Black = 2,
        /// <summary>
        /// Green color shield, Will reflect damage done to it
        /// </summary>
        Green = 3,
        /// <summary>
        /// Blue color shield, -60% flight time and -50% movement speed
        /// </summary>
        Blue = 4,
        /// <summary>
        /// Will spawn random boss before plantera, final phase of the boss, still unsure
        /// </summary>
        Rainbow = 5,
        /// <summary>
        /// Final phase
        /// </summary>
        none = 6
    }
}
