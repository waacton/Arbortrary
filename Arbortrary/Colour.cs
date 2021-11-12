namespace Wacton.Arbortrary
{
    using System;

    public class Colour
    {
        public Hsb Hsb { get; }
        public Rgb Rgb => Hsb.ToRgb();
        public double H => Hsb.H;
        public double S => Hsb.S;
        public double B => Hsb.B;
        public double A { get; }

        private Colour(double h, double s, double b, double a)
        {
            Hsb = new Hsb(h, s, b);
            A = a;
        }

        public static Colour FromHsb(double h, double s, double b, double a = 1.0) => new(h, s, b, a);

        public static Colour FromRgb(int r, int g, int b, int a = 255)
        {
            var rgb = new Rgb(r, g, b);
            var hsb = rgb.ToHsb();
            return new Colour(hsb.H, hsb.S, hsb.B, a / 255.0);
        }

        public override string ToString() => $"HSB:[{Hsb}] RGB:[{Rgb}] A:{Math.Round(A * 100, 1)}";
    }
}
