namespace gart;

public record Source
{
    public Source(FileInfo[] files)
    {
        if (files.Any(file => !file.Exists)) throw new ArgumentException("All files used as sources must exist", nameof(files));
        Files = files;
    }

    public FileInfo[] Files { get; }
}
