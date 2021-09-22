using System;
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace gart.Algorithms;

public class OverlappingDecahedrons : IAlgorithm, IGenerateWithoutSource
{
    public string Name => "Overlapping Decahedrons";
    public string Description => "This builds on Simple Boxes and gives us semi-transparent decahedron shapes over the full image. Saves as a PNG file";

    public void Generate(Dimensions dim, Destination destination)
    {
        var rand = new Random();
        const int boxSize = 20;

        using var image = new Image<Rgba32>(dim.Width, dim.Height);

        image.Mutate(ic =>
        {
            ic.Fill(Color.White);

            var rows = dim.Height / boxSize;
            var cols = dim.Width / boxSize;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var r = (byte)rand.Next(0, 255);
                    var g = (byte)rand.Next(0, 255);
                    var b = (byte)rand.Next(0, 255);
                    var squareColor = new Color(new Rgba32(r, g, b, 100));

                    var polygon = new RegularPolygon(40 * col, 40 * row, 10, 30);
                    ic.Fill(squareColor, polygon);
                }
            }
        });
        
        using var outputStream = destination.File.OpenWrite();
        image.SaveAsPng(outputStream);
    }

    public override string ToString() => Name;
}
