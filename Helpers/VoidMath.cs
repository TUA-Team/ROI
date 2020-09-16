using System;

namespace ROI.Helpers
{
    public static class VoidMath
    {
        public static ushort GetMaxVoidAffinity(VoidTier tier)
        {
            int x = (int)tier + 1;

            return (ushort)(Math.Pow(x, 2) - 16.67 * x + 60); // I'd need to redo the math for this to be 100% accurate.

            // Original values, if the formula isn't good enough for Dradon.
            /*switch (tier)
            {
                case VoidTier.Alpha:
                    return 100;
                case VoidTier.Beta:
                    return 200;
                case VoidTier.Gamma:
                    return 500;
                case VoidTier.Delta:
                    return 900;
                case VoidTier.Epsilon:
                    return 1200;
                case VoidTier.Zeta:
                    return 2000;
                default:
                    return 0;
            }*/
        }
    }
}
