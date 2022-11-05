using DDControllers;
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


ui.PrintWelcome();
imageFileService.SetDirectory(ui.AskForDirectory());

if(FolderTasksAreHandled())
{

}


//ui.PrintSummary(fc.GetDirectoryPath(), fc.GetPngCount(), fc.GetJpgCount());

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
        Console.WriteLine(folder);
    }
}


void ListAllImages()
{
    foreach(var p in repo.GetAllPhotoNames())
    {
        Console.WriteLine(p);
    }
}

void UploadImagesAndAddToStorageTable()
{
    string containerName = ui.AskForContainerName();
    blobService.CreateContainer(containerName);
    string startTrim = ui.AskForStartTrim();
    string endTrim = ui.AskForEndTrim();

    foreach(string p in repo.GetAllPhotoNames())
    {
        blobService.UploadBlob(p);
        ui.PrintString($"Uploaded {p} to {containerName}");
        var E = repo.ToEntity(repo.GetPhoto(p), containerName, startTrim, endTrim);
    
        tableService.InsertEntity(E);
    }
}

void SplitImagesOnFileType()
{
    Console.WriteLine("This are the JPG files:");
    foreach (var p in imageFileService.GetJpgs())
    {
        Console.WriteLine(p);
    }

    Console.WriteLine("This are the PNG files:");
    foreach (var p in imageFileService.GetPngs())
    {
        Console.WriteLine(p);
    }
}

bool FolderTasksAreHandled()
{
    FolderMenu FolderMenuChoice;
    do
    {
        FolderMenuChoice = ui.AskFolderMenuChoice();
        switch (FolderMenuChoice)
        {
            case FolderMenu.ListAllFolders:
            {
                PrintAllFolders();
                break;
            }
            case FolderMenu.NavigateToFolder:
            {
                
                string newDirectory = folderService.GetFolderByNum(ui.AskFolderNumber());
                imageFileService.SetDirectory(newDirectory);
                if (FileTasksAreHandled() == true)
                {
                    break;
                }else
                {
                    return false;
                }
                break;
            }
        }
    } while(FolderMenuChoice != FolderMenu.Back);
    return true;
}

bool FileTasksAreHandled()
{
    FileMenu FileMenuChoice;
    do
    {
        FileMenuChoice = ui.AskFileMenuChoice();
        InitializeRepo();
        switch (FileMenuChoice)
        {
            case FileMenu.ListAllImages:
            {
                ListAllImages();
                break;
            }
            case FileMenu.UploadImages:
            {
                UploadImagesAndAddToStorageTable();
                break;
            }
            case FileMenu.SplitImagesOnFileType:
            {
                SplitImagesOnFileType();
                break;
            }
        }
    }while(FileMenuChoice != FileMenu.Back);
    EmptyRepo();
    return true;
}