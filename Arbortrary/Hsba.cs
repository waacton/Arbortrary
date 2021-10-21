using System;
using System.Globalization;
using ColorMine.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Wacton.Arbortrary
{
    internal class Hsba
    {
        public Hsb Hsb { get; }
        public double H => Hsb.H;
        public double S => Hsb.S;
        public double B => Hsb.B;
        public double A { get; }

        public Hsba(double h, double s, double b, double a)
        {
            Hsb = new Hsb { H = h, S = s, B = b };
            A = a;
        }

        public Hsba(Hsb hsb, double a)
        {
            Hsb = hsb;
            A = a;
        }

        public Rgba32 ToRgba32()
        {
            var rgb = Hsb.ToRgb();
            var rByte = Convert.ToByte(rgb.R);
            var gByte = Convert.ToByte(rgb.G);
            var bByte = Convert.ToByte(rgb.B);
            var aByte = Convert.ToByte(A);
            return new Rgba32(rByte, gByte, bByte, aByte);
        }

        public override string ToString()
        {
            var rgb = Hsb.ToRgb();
            var r = Math.Round(rgb.R).ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
            var g = Math.Round(rgb.G).ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
            var b = Math.Round(rgb.B).ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
            var a = Math.Round(A).ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
            return $"RGB:{r},{g},{b} + A:{a}";
        }
    }
}
