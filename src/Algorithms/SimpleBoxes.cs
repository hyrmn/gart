using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace gart.Algorithms;

public class SimpleBoxes : IAlgorithm, IGenerateWithoutSource
{
    public string Name => "Simple Boxes";
    public string Description => "Generate randomly colored 20px by 20px boxes. Saves as a PNG file";

    public void Generate(Dimensions dim, Destination destination)
    {
        const int boxSize = 40;

        var rand = new Random();

        var rows = dim.Height / boxSize;
        var cols = dim.Width / boxSize;
        var halfSize = boxSize / 2;

        using var image = new Image<Rgba32>(dim.Width, dim.Height);

        image.Mutate(ic =>
        {
            ic.Fill(Color.White);

            var rotation = GeometryUtilities.DegreeToRadian(45);

            for (var row = 1; row < rows; row++)
            {
                for (var col = 1; col < cols; col++)
                {
                    var r = (byte)rand.Next(0, 255);
                    var g = (byte)rand.Next(0, 255);
                    var b = (byte)rand.Next(0, 255);
                    var squareColor = new Color(new Rgba32(r, g, b, 255));

                    var polygon = new RegularPolygon(boxSize * col, boxSize * row, 4, halfSize, rotation);
                    ic.Fill(squareColor, polygon);
                }
            }
        });
        
        using var outputStream = destination.File.OpenWrite();
        image.SaveAsPng(outputStream);
    }

    public override string ToString() => Name;
}
