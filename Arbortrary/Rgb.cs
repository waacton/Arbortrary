namespace Wacton.Arbortrary
{
    using System;
    using System.Linq;

    public class Rgb
    {
        public double R { get; }
        public double G { get; }
        public double B { get; }

        public Rgb(double r, double g, double b)
        {
            Check(r, 0.0, 1.0, "Red");
            Check(g, 0.0, 1.0, "Green");
            Check(b, 0.0, 1.0, "Blue");

            R = r;
            G = g;
            B = b;
        }

        public Rgb(int r, int g, int b)
        {
            Check(r, 0, 255, "Red");
            Check(g, 0, 255, "Green");
            Check(b, 0, 255, "Blue");

            R = r / 255.0;
            G = g / 255.0;
            B = b / 255.0;
        }

        // see https://en.wikipedia.org/wiki/HSL_and_HSV#From_RGB for details
        public Hsb ToHsb()
        {
            var components = new[] { R, G, B };
            var xMax = components.Max();
            var xMin = components.Min();
            var chroma = xMax - xMin;

            double hue;
            if (chroma == 0.0) hue = 0;
            else if (xMax == R) hue = 60 * (0 + ((G - B) / chroma));
            else if (xMax == G) hue = 60 * (2 + ((B - R) / chroma));
            else if (xMax == B) hue = 60 * (4 + ((R - G) / chroma));
            else throw new InvalidOperationException();
            hue = hue < 0 ? 360 + hue : hue;
            var brightness = xMax;
            var saturation = brightness == 0 ? 0 : chroma / brightness;
            return new Hsb(hue, saturation, brightness);
        }

        public override string ToString() => $"{Math.Round(R * 255)} {Math.Round(G * 255)} {Math.Round(B * 255)}";

        private static void Check(double value, double min, double max, string name)
        {
            if (value < min) throw new InvalidOperationException($"{name} cannot be less than {min}");
            if (value > max) throw new InvalidOperationException($"{name} cannot be more than {max}");
        }
    }
}
