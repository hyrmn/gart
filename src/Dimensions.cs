namespace gart;

public record Dimensions
{
    public Dimensions(int width, int height)
    {
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width), "Width must be a positive value");
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height), "Height must be a positive value");
        Width = width;
        Height = height;
    }

    public int Width { get; }
    public int Height { get; }
}
