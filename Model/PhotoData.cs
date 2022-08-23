public record Photo
{
    public string FileName { get; set; } ="";

    public string URI { get; set; }
    public string? ALT {get; set;}

}

public class PhotoData
{
    public PhotoData()
    {

    }
    private List<Photo> Photos { get; set; } = new List<Photo>();
    public List<string> AllPhotoNames => 
                                            Photos.Select(p => p.FileName)
                                            .ToList();

    public List<string> ALLPhotoALTS => 
                                        Photos.Select(p => p.ALT)
                                        .ToList();

    public void AddPhoto(string fileName)
    {
        Photos.Add(new Photo { FileName = fileName });
    }

    public void AddPhotos(List<string> fileNames)
    {
        foreach (string fileName in fileNames)
        {
            Photos.Add(new Photo { FileName = fileName });
        }
    }

    public TableEntity ToEntity(string year, Photo p)
    {
        string partitionKey = year;
        string rowKey = $"{year}-{p.FileName}";

        var entity = new TableEntity(partitionKey, rowKey)
        {
            {"FileName", p.FileName},
            {"URI", p.URI},
            {"ALT", p.ALT}
        };

        return entity;

    }

}