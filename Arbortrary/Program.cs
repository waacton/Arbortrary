namespace Wacton.Arbortrary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommandLine;
    using Wacton.Unicolour;
    using PointF = SixLabors.ImageSharp.PointF;

    public class Program
    {
        public static void Main(string[] args) => Parser.Default.ParseArguments<Options>(args).WithParsed(Execute);

        private static void Execute(Options options)
        {
            var fallbackOutput = GetOutputFilename(options.Seed, options.Text, options.InputFilepath);
            var targetFilepath = options.OutputFilepath ?? fallbackOutput;

            var generatedImage = GenerateTreeImage(options);
            var actualFilepath = generatedImage.Save(targetFilepath);
            Console.Write($"Arbortrary image saved to: {actualFilepath}");
        }

        public static GeneratedImage GenerateTreeImage(Options options)
        {
            var (seed, source) = Seed.Get(options.Seed, options.Text, options.InputFilepath);
            var random = new Random(seed);

            var fallbackBackground = random.GetColour(false);
            var background = !string.IsNullOrEmpty(options.Background) ? Unicolour.FromHex(options.Background) : fallbackBackground;

            var nodes = new List<Node>();
            var firstNode = GetFirstNode(options, background, random);
            nodes.Add(firstNode);

            PrintDetails(options, seed, source, background, firstNode);

            var generatedImage = new GeneratedImage(options.Width, options.Height, options.Zoom, options.CreateGif);
            generatedImage.SetBackground(background);
            generatedImage.AddCircle(firstNode.Point, firstNode.Colour);

            for (var i = 1; i < options.NodeCount; i++)
            {
                var connectedIndex = random.Next(nodes.Count);
                var connectedNode = nodes[connectedIndex];

                var node = GetNextNode(connectedNode, options.AdjustAlpha, options.Zoom, random);
                nodes.Add(node);

                generatedImage.AddLine(node.Point, node.Colour, connectedNode.Point, connectedNode.Colour);
                generatedImage.AddCircle(node.Point, node.Colour);

            }

            return generatedImage;
        }
        
        private static Node GetNextNode(Node connectedNode, bool adjustAlpha, float zoom, Random random)
        {
            var (point, bearing) = Calculations.NextPointAndBearing(connectedNode.Point, connectedNode.Bearing, zoom, random);
            var colour = Calculations.NextColour(connectedNode.Colour, adjustAlpha, random);
            return new Node(point, colour, bearing);
        }

        private static Node GetFirstNode(Options options, Unicolour background, Random random)
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

        private static Unicolour GetFirstColour(string optionalColourString, bool matchesBackground, Unicolour background, bool adjustAlpha, Random random)
        {
            Unicolour optionalColour = null;
            if (!string.IsNullOrEmpty(optionalColourString) && matchesBackground)
            {
                throw new InvalidOperationException("First colour cannot match the background when a colour is set");
            }

            if (!string.IsNullOrEmpty(optionalColourString))
            {
                optionalColour = Unicolour.FromHex(optionalColourString);
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
            var fallbackBearing = isCustomLocation ? Calculations.OptimalBearing(firstPoint, width, height) : 0;
            return optionalBearing ?? fallbackBearing;
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

        private static void PrintDetails(Options options, int seed, string seedSource, Unicolour background, Node firstNode)
        {
            Console.WriteLine("Arbortrarily generating using...");
            Console.WriteLine($"    - seed {seed} (from {seedSource})");
            Console.WriteLine($"    - width {options.Width} x height {options.Height}");
            Console.WriteLine($"    - background {background}");
            Console.WriteLine($"    - first node {firstNode.Colour} @ ({firstNode.Point.X},{firstNode.Point.Y}) @ {firstNode.Bearing} degrees");
            Console.WriteLine($"    - node count {options.NodeCount}");
            Console.WriteLine($"    - alpha {(options.AdjustAlpha ? "adjusting" : "ignored")}");
            Console.WriteLine($"    - zoom {options.Zoom}");
            Console.WriteLine($"    - gif {(options.CreateGif ? "included" : "ignored")}");
        }
    }
}
