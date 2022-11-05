public class FileCounter
{
    public int JpgCount { get; private set; }    
    public int PngCount { get; private set; }
    public int OtherCount { get; private set; }
    public int TotalCount => JpgCount + PngCount + OtherCount;

    public void CountFiles(string[] files)
    {  
        ResetCounts();
        foreach (var file in files)
        {
            var ext = Path.GetExtension(file);
            if (ext == ".jpg")
            {
                JpgCount++;
            }
            else if (ext == ".png")
            {
                PngCount++;
            }
            else
            {
                OtherCount++;
            }
        }
    }

    public string GetSummary(string directoryPath) => $"In {directoryPath} there are {PngCount} png files, {JpgCount} jpg files and {OtherCount} other files";
 
    private void ResetCounts()
    {
        JpgCount = 0;
        PngCount = 0;
        OtherCount = 0;
    }

}