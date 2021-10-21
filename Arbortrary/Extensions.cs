using System;
using SixLabors.ImageSharp.PixelFormats;

namespace Wacton.Arbortrary
{
    internal static class Extensions
    {
        public static Colour GetColour(this Random random, bool includeAlpha)
        {
            // H = 0-359, S = 0-1, B = 0-1, A = 0-1
            var h = random.NextDouble() * 359;
            var s = random.NextDouble();
            var b = random.NextDouble();
            var a = random.NextDouble();
            var alpha = includeAlpha ? a : 1.0;

            return Colour.FromHsb(h, s, b, alpha);
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

        public static Rgba32 ToRgba32(this Colour colour)
        {
            var rgb = colour.Rgb;
            var rByte = Convert.ToByte(rgb.R * 255);
            var gByte = Convert.ToByte(rgb.G * 255);
            var bByte = Convert.ToByte(rgb.B * 255);
            var aByte = Convert.ToByte(colour.A * 255);
            return new Rgba32(rByte, gByte, bByte, aByte);
        }
    }
}
