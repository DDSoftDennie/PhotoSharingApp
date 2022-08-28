public static class PhotoFactory
{
    private static List<Photo> Photos { get; set; } = new List<Photo>();

    public static Photo GetPhoto(string FileName) => 
                                                Photos
                                                .FirstOrDefault(p => p.FileName == FileName);
    public static List<string> GetAllPhotoNames() => 
                                            Photos.Select(p => p.FileName)
                                            .ToList();

    public static List<string> GetALLPhotoALTS() => 
                                        Photos.Select(p => p.ALT)
                                        .ToList();

    public static void LoadPhoto(string fileName)
    {
        Photos.Add(new Photo { FileName = fileName });
    }

    public static void LoadPhotos(List<string> fileNames)
    {
        foreach (string fileName in fileNames)
        {
            Photos.Add(new Photo { FileName = fileName });
        }
    }

    public static TableEntity ToEntity(string year, Photo p)
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