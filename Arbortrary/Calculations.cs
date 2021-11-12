namespace Wacton.Arbortrary
{
    using System;
    using SixLabors.ImageSharp;

    internal static class Calculations
    {
        private const float DistanceMin = 10;
        private const float DistanceMax = 190;

        public static (PointF point, double bearing) NextPointAndBearing(PointF oldPoint, double oldBearing, float zoom, Random random)
        {
            var degree = (random.NextDouble() * 90) - 45;
            var bearing = Modulo(oldBearing + degree, 360);

            var distance = (random.NextDouble() * DistanceMax * zoom) + (DistanceMin * zoom); // somewhere between 10 - 200 pixels away
            var xDistance = Math.Sin(ToRadians(bearing)) * distance;
            var yDistance = Math.Cos(ToRadians(bearing)) * distance;
            var x = (float)(oldPoint.X + xDistance);
            var y = (float)(oldPoint.Y - yDistance); // inverse as 0 is top for graphics, but I was positive to go upwards
            var point = new PointF(x, y);

            return (point, bearing);
        }

        public static Colour NextColour(Colour oldColour, bool adjustAlpha, Random random)
        {
            var hPositive = random.GetBool();
            var hStep = random.NextDouble() * (360 / 8.0); // hue can jump by 1/8 of the space, between 0 to 45 degrees shift
            var h = hPositive ? Modulo(oldColour.H + hStep , 360) : Modulo(oldColour.H - hStep, 360);
            h = h < 0 ? 360 + h : h;

            var sPositive = random.GetBool(oldColour.S, 0, 1);
            var sStep = random.NextDouble() * (1 / 5.0); // saturation can jump by 1/5 of the space, between 0 to 0.2 value shift
            var s = sPositive ? Math.Min(oldColour.S + sStep, 1) : Math.Max(oldColour.S - sStep, 0);

            var bPositive = random.GetBool(oldColour.B, 0, 1);
            var bStep = random.NextDouble() * (1 / 5.0); // brightness can jump by 1/5 of the space, between 0 to 0.2 value shift
            var b = bPositive ? Math.Min(oldColour.B + bStep, 1) : Math.Max(oldColour.B - bStep, 0);

            var aPositive = random.GetBool(oldColour.A, 0, 1);
            var aStep = random.NextDouble() * (1 / 5.0); // alpha can jump by 1/5 of the space, between 0 to 0.2 value shift
            var a = aPositive ? Math.Min(oldColour.A + aStep, 1) : Math.Max(oldColour.A - aStep, 0);

            return Colour.FromHsb(h, s, b, adjustAlpha ? a : oldColour.A);
        }

        public static double OptimalBearing(PointF point, int width, int height)
        {
            var halfWidth = width / 2.0;
            var halfHeight = height / 2.0;

            var isXBeyondMid = point.X > halfWidth;
            var isYBeyondMid = point.Y > halfHeight;

            var xDistance = isXBeyondMid ? point.X : width - point.X;
            var yDistance = isYBeyondMid ? point.Y : height - point.Y;

            return isXBeyondMid switch
            {
                false when isYBeyondMid => ToDegrees(Math.Tanh(xDistance / yDistance)),
                false when !isYBeyondMid => ToDegrees(Math.Tanh(yDistance / xDistance)) + 90,
                true when !isYBeyondMid => ToDegrees(Math.Tanh(xDistance / yDistance)) + 180,
                true when isYBeyondMid => ToDegrees(Math.Tanh(yDistance / xDistance)) + 270,
                _ => throw new InvalidOperationException("Cannot calculate bearing")
            };
        }
        
        private static double Modulo(double value, double modulus)
        {
            var remainder = value % modulus;
            return remainder < 0 ? modulus + remainder : remainder; // handles negatives, e.g. -10 % 360 returns 350 instead of -10
        }
        
        private static double ToRadians(double degrees) => Math.PI * degrees / 180;
        private static double ToDegrees(double radians) => radians * 180 / Math.PI;
    }
}