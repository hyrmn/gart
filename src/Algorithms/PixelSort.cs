namespace gart.Algorithms;

public class PixelSort : IAlgorithm, IGenerateWithSource
{
    public string Name => "Sort pixels by value";
    public string Description => "Goes line-by-line-through an image and sorts the pixels in each line by the sum of their RGB value";

    public void Generate(string[] sourceFiles, int width, int height, string destinationFile)
    {
        Console.WriteLine("Yay");
    }

    public override string ToString() => Name;
}
