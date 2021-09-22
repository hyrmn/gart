namespace gart.Algorithms;

public interface IGenerateWithSource
{
    bool AllowMultipleFiles { get; }

    void Generate(Source source, Dimensions destinationDimensions, Destination destination);
}
