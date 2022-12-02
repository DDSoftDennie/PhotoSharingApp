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
string rootDirectory = "";

ui.PrintWelcome();
rootDirectory = ui.AskForDirectory();
imageFileService.SetDirectory(rootDirectory);

bool folderTasksArehandled = false;
while (!folderTasksArehandled)
{
    folderTasksArehandled = FolderTasksAreHandled();
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
            bool filesAreHandled = false;
            while (!filesAreHandled)
            {
                filesAreHandled = FileTasksAreHandled();
            }
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
            UploadImagesAndAddToStorageTable();
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