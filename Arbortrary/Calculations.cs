namespace Wacton.Arbortrary
{
    using System;
    using SixLabors.ImageSharp;
    using Wacton.Unicolour;

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

        public static Unicolour NextColour(Unicolour oldColour, bool adjustAlpha, Random random)
        {
            var oldHsb = oldColour.Hsb;
            var oldAlpha = oldColour.Alpha;
            
            var hPositive = random.GetBool();
            var hStep = random.NextDouble() * (360 / 8.0); // hue can jump by 1/8 of the space, between 0 to 45 degrees shift
            var h = hPositive ? Modulo(oldHsb.H + hStep , 360) : Modulo(oldHsb.H - hStep, 360);
            h = h < 0 ? 360 + h : h;

            var sPositive = random.GetBool(oldHsb.S, 0, 1);
            var sStep = random.NextDouble() * (1 / 5.0); // saturation can jump by 1/5 of the space, between 0 to 0.2 value shift
            var s = sPositive ? Math.Min(oldHsb.S + sStep, 1) : Math.Max(oldHsb.S - sStep, 0);

            var bPositive = random.GetBool(oldHsb.B, 0, 1);
            var bStep = random.NextDouble() * (1 / 5.0); // brightness can jump by 1/5 of the space, between 0 to 0.2 value shift
            var b = bPositive ? Math.Min(oldHsb.B + bStep, 1) : Math.Max(oldHsb.B - bStep, 0);

            var aPositive = random.GetBool(oldAlpha.A, 0, 1);
            var aStep = random.NextDouble() * (1 / 5.0); // alpha can jump by 1/5 of the space, between 0 to 0.2 value shift
            var a = aPositive ? Math.Min(oldAlpha.A + aStep, 1) : Math.Max(oldAlpha.A - aStep, 0);

            return Unicolour.FromHsb(h, s, b, adjustAlpha ? a : oldAlpha.A);
        }

        public static double OptimalBearing(PointF point, int width, int height)
        {
            var halfWidth = width / 2.0;
            var halfHeight = height / 2.0;

            var (x, y) = point;
            var isXBeyondMid = x > halfWidth;
            var isYBeyondMid = y > halfHeight;

            var xDistance = isXBeyondMid ? x : width - x;
            var yDistance = isYBeyondMid ? y : height - y;

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