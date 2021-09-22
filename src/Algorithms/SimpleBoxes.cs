namespace gart.Algorithms;

public class SimpleBoxes : IAlgorithm, IGenerateWithoutSource
{
    public string Name => "Simple Boxes";
    public string Description => "Generate randomly colored 20px by 20px boxes";

    public void Generate(int width, int height, string destinationFile)
    {
        Console.WriteLine("Yay");
    }

    public override string ToString() => Name;
}
