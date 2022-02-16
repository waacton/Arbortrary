namespace Wacton.Arbortrary
{
    using System.Linq;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Drawing;
    using SixLabors.ImageSharp.Drawing.Processing;
    using SixLabors.ImageSharp.Formats.Gif;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp.Processing.Processors.Quantization;
    using Wacton.Unicolour;
    using Path = System.IO.Path;

    public class GeneratedImage
    {
        private const float EllipseRadius = 3;
        private const float LineThickness = 1;
        
        public Image Png { get; }
        public Image Gif { get; }
        public bool HasGif => Gif != null;
        public float Zoom { get; }

        private bool hasInitialGifFrame = true;
        
        public GeneratedImage(int width, int height, float zoom, bool createGif)
        {
            Png = new Image<Rgba32>(width, height);
            Gif = createGif ? new Image<Rgba32>(width, height) : null;
            Zoom = zoom;
        }
        
        public void SetBackground(Unicolour colour)
        {
            Png.Mutate(x => x.BackgroundColor(colour.ToRgba32()));
            AddGifFrame();
        }

        public void AddCircle(PointF point, Unicolour colour)
        {
            IBrush brush = new SolidBrush(colour.ToRgba32());
            var radius = EllipseRadius * Zoom;
            IPath ellipse = new EllipsePolygon(point, radius);
            Png.Mutate(x => x.Fill(brush, ellipse));
            AddGifFrame();
        }

        public void AddLine(PointF point1, Unicolour colour1, PointF point2, Unicolour colour2)
        {
            var colorStop1 = new ColorStop(0.0f, colour1.ToRgba32());
            var colorStop2 = new ColorStop(1.0f, colour2.ToRgba32());
            var thickness = LineThickness * Zoom;
            IBrush brush = new LinearGradientBrush(point1, point2, GradientRepetitionMode.DontFill, colorStop1, colorStop2);
            Png.Mutate(x => x.DrawLines(brush, thickness, point1, point2));
            AddGifFrame();
        }

        private void AddGifFrame()
        {
            if (HasGif)
            {
                Gif.Frames.AddFrame(Png.Frames.Last());
            }
        }
        
        private void RemoveInitialGifFrame()
        {
            if (!HasGif || !hasInitialGifFrame)
            {
                return;
            }
            
            Gif.Frames.RemoveFrame(0);
            hasInitialGifFrame = false;
        }
        
        public string Save(string filepath)
        {
            var filepathWithoutExtension = Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath));
            Png.SaveAsPng($"{filepathWithoutExtension}.png");

            if (HasGif)
            {
                RemoveInitialGifFrame();
                var encoder = new GifEncoder
                {
                    ColorTableMode = GifColorTableMode.Local,
                    Quantizer = new OctreeQuantizer(new QuantizerOptions { Dither = null })
                };
                
                Gif.SaveAsGif($"{filepathWithoutExtension}.gif", encoder);
            }

            return filepathWithoutExtension;
        }
    }
}