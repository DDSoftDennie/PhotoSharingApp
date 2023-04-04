using DDControllers;
using PhotoSharingApp.Factories;
using PhotoSharingApp.Model;

var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.local.json");
var configuration = builder.Build();
var connectionString = configuration.GetConnectionString("StorageAccount");               
var ui = new UIController();
var repo = new PhotoRepository();
var imageFileService = new ImageFileService();
var folderService = new FolderService();
var blobService = new DDBlobService(connectionString);
var tableService = new DDTableService(connectionString, "conferencepictures");
var formatFactory = new FormatFactory("C:\\Users\\dpcfr\\Documents\\Afbeeldingen\\format.csv");
string rootDirectory = "";
rootDirectory = "C:\\Users\\dpcfr\\Documents\\Afbeeldingen\\";
imageFileService.SetDirectory(rootDirectory);
Format format = formatFactory.getFormat();
bool folderTasksAreHandled = false;

ui.PrintWelcome();
ui.PrintDirectory(rootDirectory);

while (!folderTasksAreHandled)
{
    folderTasksAreHandled = FolderTasksAreHandled();
} 

bool FolderTasksAreHandled()
{
    FolderMenu FolderMenuChoice = ui.AskFolderMenuChoice();
    switch (FolderMenuChoice)
    {
        case FolderMenu.ListAllFolders:
        {
            PrintAllFolders();
            return false;
        }
        case FolderMenu.NavigateToFolder:
        {
            string newDirectory = folderService.GetFolderByNum(ui.AskFolderNumber());
            imageFileService.SetDirectory(newDirectory);
            HandleFiles();
            return false;
        }
        case FolderMenu.ChangeFolder:
        {
            var dir = ui.AskForDirectory();
            imageFileService.SetDirectory(dir);
            HandleFiles();
            return false;
        }
        case FolderMenu.Back:
        {
            imageFileService.SetDirectory(rootDirectory);
            return true;
        }
    }
    return false;
}

void HandleFiles()
{
    bool filesAreHandled = false;
    while (!filesAreHandled)
    {
        filesAreHandled = FileTasksAreHandled();
    }
}

bool FileTasksAreHandled()
{
    FileMenu FileMenuChoice = ui.AskFileMenuChoice();
    InitializeRepo();
    switch (FileMenuChoice)
    {
        case FileMenu.ListAllImages:
        {
            ListAllImages();
            return false;
        }
        case FileMenu.UploadImages:
        {
            AddImagesToStorageTable(UploadImagesAndGetStorageInfo());
            return false;
        }
        case FileMenu.SplitImagesOnFileType:
        {
            SplitImagesOnFileType();
            return false;
        }
        case FileMenu.Back:
        {
            EmptyRepo();
            imageFileService.SetDirectory(rootDirectory);
            return true;
        }
    }
    return false;
}

void InitializeRepo()
{
    repo = new PhotoRepository(imageFileService.GetPhotoFiles().ToList());
}

void EmptyRepo()
{
    repo.Empty();
}

void PrintAllFolders()
{
    var folders = folderService.GetFoldersWithNum();
    foreach (var folder in folders)
    {
        WriteLine(folder);
    }
}

void ListAllImages()
{
    foreach(var p in repo.GetAllPhotoNames())
    {
        WriteLine(p);
    }
}

(string?, string?, string?) UploadImagesAndGetStorageInfo()
{
    if(repo != null && blobService != null)
    {
        string containerName = ui.AskForContainerName();
        blobService.CreateContainer(containerName);
        string startTrim = ui.AskForStartTrim(format.StartTrim.ToString());
        string endTrim = ui.AskForEndTrim(format.EndTrim.ToString());

        foreach (string p in repo.GetAllPhotoNames())
        {
            blobService.UploadBlob(p);
            WriteLine($"Uploaded {p} to {containerName}");
        }
        return (containerName, startTrim, endTrim);
    }
    else
    {
        WriteLine("Error with uploading!");
    }
    return (null, null, null);

}

void AddImagesToStorageTable((string?, string?,string?) storageInfo)
{
    if(repo != null && tableService != null)
    {
        if(storageInfo.Item1 != null && storageInfo.Item2 != null && storageInfo.Item3 != null)
        {
            foreach (string p in repo.GetAllPhotoNames())
            {
                var E = repo.ToEntity(repo.GetPhoto(p), storageInfo.Item1, storageInfo.Item2, storageInfo.Item3);
                tableService.InsertEntity(E);
            }
            WriteLine("Added images to storage table");
        }
        else
        {
            WriteLine("Error adding images to storage table");
        }
    }
}

void SplitImagesOnFileType()
{
    WriteLine("This are the JPG files:");
    foreach (var p in imageFileService.GetJpgs())
    {
        WriteLine(p);
    }

    WriteLine("This are the PNG files:");
    foreach (var p in imageFileService.GetPngs())
    {
        WriteLine(p);
    }
}