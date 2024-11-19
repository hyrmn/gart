using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace gart.Algorithms;

public class RandomGranite : IAlgorithm, IGenerateWithSource
{
    public string Name => "Random shapes colored from source";
    public string Description => "Chooses a random pixel from the source image and then draws a polygon of that color on the destination.";
    public bool AllowMultipleFiles => false;

    private const int iterations = 5000;

    private const float shapeReductionFactor = 0.002f;
    private const float startingAlphaTransparency = 0.1f;
    private const float alphaTransparancyIncreaseStep = 0.06f;

    private const int polyMinSides = 3;
    private const int polyMaxSides = 16;

    public void Generate(Source source, Dimensions dim, Destination destination)
    {
        var rand = new Random();

        var initialShapeSize = 0.75 * dim.Height;

        using var srcImg = Image.Load<Rgb24>(source.Files.First().FullName);
        var srcWidth = srcImg.Size.Width;
        var srcHeight = srcImg.Size.Height;

        using var destImg = new Image<Rgba32>(dim.Width, dim.Height);

        destImg.Mutate<Rgba32>(ic =>
        {
            ic.Fill(Color.Black);
            var alpha = startingAlphaTransparency;
            var strokeSize = initialShapeSize;
            for(var i = 0; i < iterations; i++)
            {
                var srcX = rand.Next(0, srcWidth);
                var srcY = rand.Next(0, srcHeight);
                var destRgba = new Rgba32(srcImg[srcX, srcY].R, srcImg[srcX, srcY].G, srcImg[srcX, srcY].B, (byte)alpha);
                var edges = rand.Next(polyMinSides, polyMaxSides);
                var x = rand.NextDouble() * dim.Width;
                var y = rand.NextDouble() * dim.Height;
                var rotation = (float)(90 * rand.NextDouble());
                var polygon = new RegularPolygon((float)x, (float)y, edges, (float)strokeSize, rotation);

                ic.Fill(destRgba, polygon);
                strokeSize -= (shapeReductionFactor * strokeSize);
                alpha += alphaTransparancyIncreaseStep;
            }
        });

        using var outputStream = destination.File.OpenWrite();
        destImg.SaveAsPng(outputStream);
    }

    public override string ToString() => Name;
}
