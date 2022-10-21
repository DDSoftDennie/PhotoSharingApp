public class PhotoRepository
{
    private List<Photo> Photos { get; set; } = new List<Photo>();

    public Photo GetPhoto(string FileName) => 
                                                Photos
                                                .FirstOrDefault(p => p.FileName == FileName);
    public List<string> GetAllPhotoNames() => 
                                            Photos.Select(p => p.FileName)
                                            .ToList();

    public List<string> GetALLPhotoALTS() => 
                                        Photos.Select(p => p.ALT)
                                        .ToList();

    public void LoadPhoto(string fileName)
    {
        Photos.Add(new Photo { FileName = fileName });
    }

    public void LoadPhotos(List<string> fileNames)
    {
        foreach (string fileName in fileNames)
        {
            Photos.Add(new Photo { FileName = fileName });
        }
    }

    public TableEntity ToEntity(Photo p, string containerName, string startTrim, string endTrim)
    {
        string partitionKey = containerName;
        string photoNum = p.FileName.TrimEnd(endTrim.ToCharArray());
        photoNum = photoNum.TrimStart(startTrim.ToCharArray());
        string rowKey = photoNum;

        var entity = new TableEntity(partitionKey, rowKey)
        {
            {"FileName", p.FileName},
            {"URI", p.URI},
            {"ALT", p.ALT}
        };

        return entity;

    }

}