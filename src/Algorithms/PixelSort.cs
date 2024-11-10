using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace gart.Algorithms;

public class PixelSort : IAlgorithm, IGenerateWithSource
{
    public string Name => "Sort pixels by value";
    public string Description => "Goes line-by-line-through an image and sorts the pixels in each line by the sum of their RGB value. Destination size is ignored; the output will be the same size as the input.";
    public bool AllowMultipleFiles => false;

    public void Generate(Source source, Dimensions destinationDimensions, Destination destination)
    {
        using var srcImg = Image.Load<Rgb24>(source.Files.First().FullName);
        var srcWidth = srcImg.Size().Width;
        var srcHeight = srcImg.Size().Height;

        using var destImg = new Image<Rgb24>(srcWidth, srcHeight);

        srcImg.ProcessPixelRows(pixelAccessor =>
        {
            for (int row = 0; row < pixelAccessor.Height; row++)
            {
                var srcRow = pixelAccessor.GetRowSpan(row).ToArray();
                var orderedPixels = srcRow.OrderBy(p => p.R + p.G + p.B).ToArray();

                for (var col = 0; col < orderedPixels.Length; col++)
                {
                    destImg[col, row] = orderedPixels[col];
                }
            }
        });

        using var outputStream = destination.File.OpenWrite();
        destImg.SaveAsPng(outputStream);
    }

    public override string ToString() => Name;
}
