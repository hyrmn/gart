namespace gart.Algorithms;

public interface IGenerateWithSource
{
    void Generate(string[] sourceFiles, int width, int height, string destinationFile);
}
