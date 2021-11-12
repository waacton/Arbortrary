namespace Wacton.Arbortrary.Tests
{
    using NUnit.Framework;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    public class Regression
    {
        private Image<Rgba32> imageVersion1;

        [SetUp]
        public void Setup()
        {
            imageVersion1 = (Image<Rgba32>)Image.Load(@"Resources/Arbortrary_v1.png"); // remember to set "Copy to Output Directory"
        }

        [Test]
        public void Version1()
        {
            var options = new Options { Text = "Arbortrary", Width = 1920, Height = 1080, NodeCount = 200, Zoom = 1 };
            var generatedImage = Program.GenerateTreeImage(options);
            var png = (Image<Rgba32>) generatedImage.Png;

            for (var y = 0; y < imageVersion1.Height; y++)
            {
                var expectedRow = imageVersion1.GetPixelRowSpan(y);
                var actualRow = png.GetPixelRowSpan(y);
                Assert.That(actualRow.ToArray(), Is.EqualTo(expectedRow.ToArray()));
            }
        }
    }
}