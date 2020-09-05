using System;

namespace ROI.Void
{
    public static class VoidMath
    {
        public static ushort GetMaxVoidAffinity(VoidTiers tier)
        {
            int x = (int)tier + 1;

            return (ushort)(Math.Pow(x, 2) - 16.67 * x + 60); // I'd need to redo the math for this to be 100% accurate.

            // Original values, if the formula isn't good enough for Dradon.
            /*switch (tier)
            {
                case VoidTiers.Alpha:
                    return 100;
                case VoidTiers.Beta:
                    return 200;
                case VoidTiers.Gamma:
                    return 500;
                case VoidTiers.Delta:
                    return 900;
                case VoidTiers.Epsilon:
                    return 1200;
                case VoidTiers.Zeta:
                    return 2000;
                default:
                    return 0;
            }*/
        }
    }
}
