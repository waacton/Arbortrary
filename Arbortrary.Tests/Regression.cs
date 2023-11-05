namespace Wacton.Arbortrary.Tests;

using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Regression
{
    private Image<Rgba32> expectedImage;

    [SetUp]
    public void Setup()
    {
        expectedImage = (Image<Rgba32>)Image.Load(@"Resources/Arbortrary_v1.2.png"); // remember to set "Copy to Output Directory"
    }

    [Test]
    public void Version1()
    {
        var options = new Options { Text = "Arbortrary", Width = 1920, Height = 1080, NodeCount = 200, Zoom = 1 };
        var generatedImage = Program.GenerateTreeImage(options);
        var actualImage = (Image<Rgba32>) generatedImage.Png;
            
        expectedImage.ProcessPixelRows(actualImage, (expectedImageAccessor, actualImageAccessor) =>
        {
            for (var y = 0; y < expectedImageAccessor.Height; y++)
            {
                var expectedRow = expectedImageAccessor.GetRowSpan(y);
                var actualRow = actualImageAccessor.GetRowSpan(y);
                Assert.That(actualRow.ToArray(), Is.EqualTo(expectedRow.ToArray()));
            }
        });
    }
}