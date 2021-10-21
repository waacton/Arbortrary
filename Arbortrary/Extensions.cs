using System;
using ColorMine.ColorSpaces;

namespace Wacton.Arbortrary
{
    internal static class Extensions
    {
        public static Hsba GetColor(this Random random, bool includeAlpha)
        {
            // H = 0-359, S = 0-1, B = 0-1
            var h = random.NextDouble() * 359;
            var s = random.NextDouble();
            var b = random.NextDouble();
            var hsb = new Hsb { H = h, S = s, B = b };

            // A = 0-255
            var a = random.NextDouble() * 255;
            var alpha = includeAlpha ? a : 255;

            return new Hsba (hsb, alpha);
        }

        public static bool GetBool(this Random random)
        {
            var randomNumber = random.Next(0, 2);
            return randomNumber == 1;
        }

        public static bool GetBool(this Random random, double value, double min, double max)
        {
            var randomNumber = random.Next(0, 2); // makes sure the call to random is at least performed

            if (value == min) return true;
            if (value == max) return false;
            return randomNumber == 1;
        }
    }
}
