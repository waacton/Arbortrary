using CommandLine;

namespace Wacton.Arbortrary
{
    internal class Options
    {
        [Option('s', "seed", Group = "seed", HelpText = "Seed to use for the random number generator", MetaValue = "int")]
        public int? Seed { get; set; }

        [Option('t', "text", Group = "seed", HelpText = "Text to use as the seed for the random number generator", MetaValue = "string")]
        public string Text { get; set; }

        [Option('f', "file", Group = "seed", HelpText = "File to use as the seed for the random number generator", MetaValue = "string")]
        public string InputFilepath { get; set; }

        [Option('o', "output", Required = false, HelpText = "(Default: timestamp + seed name) Output filepath", MetaValue = "string")]
        public string OutputFilepath { get; set; }

        [Option('w', "width", Required = false, Default = 1920, HelpText = "Width of the output image", MetaValue = "int")]
        public int Width { get; set; }

        [Option('h', "height", Required = false, Default = 1080, HelpText = "Height of the output image", MetaValue = "int")]
        public int Height { get; set; }

        [Option('b', "background-colour", Required = false, HelpText = "(Default: random colour) Background colour in #ARGB format", MetaValue = "string")]
        public string Background { get; set; }

        [Option('c', "first-node-colour", Required = false, HelpText = "(Default: random colour) Set first node colour to specified #ARGB format", MetaValue = "string")]
        public string FirstColor { get; set; }

        [Option('m', "first-node-match-background", Required = false, Default = false, HelpText = "Set first node colour to match background", MetaValue = "bool")]
        public bool FirstColourMatchesBackground { get; set; }

        [Option('x', "first-node-x", Required = false, HelpText = "(Default: centre-bottom) X coordinate of first node", MetaValue = "float")]
        public float? FirstPixelX { get; set; }

        [Option('y', "first-node-y", Required = false, HelpText = "(Default: centre-bottom) Y coordinate of first node", MetaValue = "float")]
        public float? FirstPixelY { get; set; }

        [Option('d', "first-node-bearing", Required = false, HelpText = "(Default: 0 degrees, or furthest corner if coordinates provided) Bearing of first node", MetaValue = "double")]
        public double? FirstBearing { get; set; }

        [Option('n', "nodes", Required = false, Default = 200, HelpText = "Number of nodes to generate", MetaValue = "int")]
        public int NodeCount { get; set; }

        [Option('a', "alpha", Required = false, Default = false, HelpText = "Adjust alpha channel while generating", MetaValue = "bool")]
        public bool AdjustAlpha { get; set; }
    }
}
