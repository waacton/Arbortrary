namespace Wacton.Arbortrary
{
    using System;

    public class Hsb
    {
        public double H { get; }
        public double S { get; }
        public double B { get; }

        public Hsb(double h, double s, double b)
        {
            Check(h, 0.0, 360.0, "Hue");
            Check(s, 0.0, 1.0, "Saturation");
            Check(b, 0.0, 1.0, "Brightness");

            H = h;
            S = s;
            B = b;
        }

        // see https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB for details
        public Rgb ToRgb()
        {
            var chroma = B * S;
            var h = H / 60;
            var x = chroma * (1 - Math.Abs(((h % 2) - 1)));

            var (r, g, b) = h switch
            {
                < 1 => (chroma, x, 0.0),
                < 2 => (x, chroma, 0.0),
                < 3 => (0.0, chroma, x),
                < 4 => (0.0, x, chroma),
                < 5 => (x, 0.0, chroma),
                < 6 => (chroma, 0.0, x),
                _ => (0.0, 0.0, 0.0)
            };

            var m = B - chroma;
            var (red, green, blue) = (r + m, g + m, b + m);
            return new Rgb(red, green, blue);
        }

        public override string ToString() => $"{Math.Round(H, 1)}° {Math.Round(S * 100, 1)}% {Math.Round(B * 100, 1)}%";

        private static void Check(double value, double min, double max, string name)
        {
            if (value < min) throw new InvalidOperationException($"{name} cannot be less than {min}");
            if (value > max) throw new InvalidOperationException($"{name} cannot be more than {max}");
        }
    }
}
