public class FolderService
{

    public IEnumerable<string> GetFolders()
    {
        var folders = Directory.GetDirectories(".");
        foreach (var folder in folders)
        {
            yield return folder;
        }

    }

    public string GetFolderByNum(int num)
    {
        var folders = GetFolders();
        var f = folders.ElementAt(num-1);
        return f;
    }

    public IEnumerable<string> GetFoldersWithNum()
    {
        var folders =  GetFolders();
        int i = 0;
        int count = folders.Count();
        for(i = 0; i < count; i++)
        {
            yield return $"{i+1}| {folders.ElementAt(i)}";
        }
    }

}