namespace Wacton.Arbortrary
{
    using System;
    using SixLabors.ImageSharp.PixelFormats;
    using Wacton.Unicolour;

    internal static class Extensions
    {
        public static Unicolour GetColour(this Random random, bool includeAlpha)
        {
            // H = 0-359, S = 0-1, B = 0-1, A = 0-1
            var h = random.NextDouble() * 359;
            var s = random.NextDouble();
            var b = random.NextDouble();
            var a = random.NextDouble();
            var alpha = includeAlpha ? a : 1.0;

            return Unicolour.FromHsb(h, s, b, alpha);
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

        public static Rgba32 ToRgba32(this Unicolour colour)
        {
            var rgb = colour.Rgb;
            var alpha = colour.Alpha;
            return new Rgba32((float)rgb.R, (float)rgb.G, (float)rgb.B, (float)alpha.A);
        }
    }
}
