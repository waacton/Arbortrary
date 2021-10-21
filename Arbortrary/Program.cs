using System;
using System.Collections.Generic;
using System.Drawing;
using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Path = System.IO.Path;
using PointF = SixLabors.ImageSharp.PointF;

namespace Wacton.Arbortrary
{
    public class Program
    {
        public static void Main(string[] args) => Parser.Default.ParseArguments<Options>(args).WithParsed(Execute);

        private static void Execute(Options options)
        {
            var fallbackOutput = GetOutputFilename(options.Seed, options.Text, options.InputFilepath);
            var outputFilepath = options.OutputFilepath ?? fallbackOutput;

            using var image = GenerateTreeImage(options);
            SaveImage(image, outputFilepath);
        }

        public static Image<Rgba32> GenerateTreeImage(Options options)
        {
            var (seed, source) = Seed.Get(options.Seed, options.Text, options.InputFilepath);
            var random = new Random(seed);

            var fallbackBackground = random.GetColour(false);
            var background = !string.IsNullOrEmpty(options.Background) ? GetColourFromString(options.Background) : fallbackBackground;

            var nodes = new List<Node>();
            var firstNode = GetFirstNode(options, background, random);
            nodes.Add(firstNode);

            PrintDetails(options, seed, source, background, firstNode);

            var image = new Image<Rgba32>(options.Width, options.Height);
            SetBackground(image, background);
            AddCircle(image, firstNode.Point, firstNode.Colour);

            for (var i = 1; i < options.NodeCount; i++)
            {
                var connectedIndex = random.Next(nodes.Count);
                var connectedNode = nodes[connectedIndex];

                var node = GetNextNode(connectedNode, options.AdjustAlpha, random);
                nodes.Add(node);

                AddLine(image, node.Point, node.Colour, connectedNode.Point, connectedNode.Colour);
                AddCircle(image, node.Point, node.Colour);
            }

            return image;
        }

        private static Node GetFirstNode(Options options, Colour background, Random random)
        {
            if (options.FirstPixelX.HasValue ^ options.FirstPixelY.HasValue)
            {
                throw new InvalidOperationException("When specifying first pixel location, both X and Y values must be provided");
            }

            var isCustomLocation = options.FirstPixelX.HasValue; // if X has value, Y should too (otherwise crashed due to above check)

            var firstPoint = GetFirstPoint(options.FirstPixelX, options.FirstPixelY, options.Width, options.Height);
            var firstColour = GetFirstColour(options.FirstColour, options.FirstColourMatchesBackground, background, options.AdjustAlpha, random);
            var firstBearing = GetFirstBearing(options.FirstBearing, isCustomLocation, firstPoint, options.Width, options.Height);
            return new Node(firstPoint, firstColour, firstBearing);
        }

        private static PointF GetFirstPoint(float? optionalX, float? optionalY, int width, int height)
        {
            var firstX = optionalX ?? width / 2.0f;
            var firstY = optionalY ?? height;
            return new PointF(firstX, firstY);
        }

        private static Colour GetFirstColour(string optionalColourString, bool matchesBackground, Colour background, bool adjustAlpha, Random random)
        {
            Colour optionalColour = null;
            if (!string.IsNullOrEmpty(optionalColourString) && matchesBackground)
            {
                throw new InvalidOperationException("First colour cannot match the background when a colour is set");
            }

            if (!string.IsNullOrEmpty(optionalColourString))
            {
                optionalColour = GetColourFromString(optionalColourString);
            }
            else if (matchesBackground)
            {
                optionalColour = background;
            }

            var fallbackColour = random.GetColour(adjustAlpha);
            return optionalColour ?? fallbackColour;
        }

        private static double GetFirstBearing(double? optionalBearing, bool isCustomLocation, PointF firstPoint, int width, int height)
        {
            var fallbackBearing = isCustomLocation ? CalculateOptimalBearing(firstPoint, width, height) : 0;
            return optionalBearing ?? fallbackBearing;
        }

        private static Colour GetColourFromString(string value)
        {
            var color = ColorTranslator.FromHtml(value);
            return Colour.FromRgb(color.R, color.G, color.B, color.A);
        }

        private static Node GetNextNode(Node connectedNode, bool adjustAlpha, Random random)
        {
            var degree = (random.NextDouble() * 90) - 45;
            var bearing = Modulo(connectedNode.Bearing + degree, 360);

            var distance = (random.NextDouble() * 190) + 10; // somewhere between 10 - 200 pixels away
            var xDistance = Math.Sin(ToRadians(bearing)) * distance;
            var yDistance = Math.Cos(ToRadians(bearing)) * distance;
            var x = connectedNode.Point.X + xDistance;
            var y = connectedNode.Point.Y - yDistance; // inverse as 0 is top for graphics, but I was positive to go upwards
            var point = new PointF((float)x, (float)y);

            var oldColour = connectedNode.Colour;

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

            var colour = Colour.FromHsb(h, s, b, adjustAlpha ? a : oldColour.A);

            return new Node(point, colour, bearing);
        }

        
        private static double Modulo(double value, double modulus)
        {
            var remainder = value % modulus;
            return remainder < 0 ? modulus + remainder : remainder; // handles negatives, e.g. -10 % 360 returns 350 instead of -10
        }

        private static double CalculateOptimalBearing(PointF point, int width, int height)
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

        private static double ToRadians(double degrees) => Math.PI * degrees / 180;
        private static double ToDegrees(double radians) => radians * 180 / Math.PI;

        private static void SetBackground(Image image, Colour colour)
        {
            image.Mutate(x => x.BackgroundColor(colour.ToRgba32()));
        }

        private static void AddCircle(Image image, PointF point, Colour colour)
        {
            IBrush brush = new SolidBrush(colour.ToRgba32());
            IPath ellipse = new EllipsePolygon(point, 3);
            image.Mutate(x => x.Fill(brush, ellipse));
        }

        private static void AddLine(Image image, PointF point1, Colour colour1, PointF point2, Colour colour2)
        {
            var colorStop1 = new ColorStop(0.0f, colour1.ToRgba32());
            var colorStop2 = new ColorStop(1.0f, colour2.ToRgba32());
            IBrush brush = new LinearGradientBrush(point1, point2, GradientRepetitionMode.DontFill, colorStop1, colorStop2);
            image.Mutate(x => x.DrawLines(brush, 1.0f, point1, point2));
        }

        private static string GetOutputFilename(int? seed, string text, string filepath)
        {
            var date = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss");
            string details;
            if (seed.HasValue) details = seed.ToString();
            else if (!string.IsNullOrEmpty(text)) details = text;
            else if (!string.IsNullOrEmpty(filepath)) details = Path.GetFileName(filepath);
            else { throw new InvalidOperationException("No seed available"); }
            return string.IsNullOrEmpty(details) ? $"{date}.png" : $"{date}_{details}.png";
        }

        private static void PrintDetails(Options options, int seed, string seedSource, Colour background, Node firstNode)
        {
            Console.WriteLine("Arbortrarily generating using...");
            Console.WriteLine($"    - seed {seed} (from {seedSource})");
            Console.WriteLine($"    - width {options.Width} x height {options.Height}");
            Console.WriteLine($"    - background {background}");
            Console.WriteLine($"    - first node {firstNode.Colour} @ ({firstNode.Point.X},{firstNode.Point.Y}) @ {firstNode.Bearing} degrees");
            Console.WriteLine($"    - node count {options.NodeCount}");
            Console.WriteLine($"    - alpha {(options.AdjustAlpha ? "adjusting" : "ignored")}");
        }

        private static void SaveImage(Image image, string filepath)
        {
            image.SaveAsPng(filepath);
            Console.Write($"Arbortrary image saved to: {filepath}");
        }
    }
}
