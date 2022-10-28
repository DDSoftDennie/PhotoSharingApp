using DDControllers;


var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.local.json");
var configuration = builder.Build();
var connectionString = configuration.GetConnectionString("StorageAccount");               


var ui = new UIController();
var fc = new FileController(ui.GetDirectory());
var blobService = new DDBlobService(connectionString);
var tableService = new DDTableService(connectionString, "conferencepictures");

var MenuChoice = ui.AskMainMenu();

while (MenuChoice != MainMenuChoice.Exit)
{
    switch (MenuChoice)
    {
        case MainMenuChoice.ListAllFolders:
        {
            PrintAllFolders();
            break;
        }
        case MainMenuChoice.NavigateToFolder:
        {
            fc = new FileController(fc.GetFolderByNum(ui.AskFolderNumber()));
            MenuChoice = MainMenuChoice.Exit;
            continue;
        }
    }
    MenuChoice = ui.AskMainMenu();
}



PhotoRepository repo = new PhotoRepository();
repo.LoadPhotos(fc.GetPhotoFiles().ToList());
ui.PrintSummary(fc.GetDirectoryPath(), fc.GetPngCount(), fc.GetJpgCount());

var Option = ui.AskOptions();
while (Option != Options.Exit)
{
    switch (Option)
    {
        case Options.ListAllImages:
        {
            ListAllImages();
            break;
        }
        case Options.UploadImages:
        {
            UploadImagesAndAddToStorageTable();
            break;
        }
        case Options.SplitImagesOnFileType:
        {
            SplitImagesOnFileType();
            break;
        }
    }
    Option = ui.AskOptions();
}

void PrintAllFolders()
{
    var folders = fc.GetFoldersWithNum();
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
    ui.PrintString("This are the JPG files:");
    foreach (var p in fc.GetJpgs())
    {
        ui.PrintString(p);
    }
    ui.PrintString("This are the PNG files:");
    foreach (var p in fc.GetPngs())
    {
        ui.PrintString(p);
    }
}
